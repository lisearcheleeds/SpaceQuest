using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CoroutineベースのScheduler
/// Jackpot.Schedulerの使い方ほぼ一緒
/// </summary>
public class Scheduler : MonoSingleton<Scheduler>
{
    /// <summary>
    /// 現在動いているワーカーリスト
    /// </summary>
    List<IScheduleWorker> workerList = new List<IScheduleWorker>();

    /// <summary>
    /// Worker state.
    /// </summary>
    public enum WorkerState
    {
        /// <summary>
        /// 待機状態を示します
        /// </summary>
        Waiting,

        /// <summary>
        /// タスクを実行している状態を示します
        /// </summary>
        Working,

        /// <summary>
        /// 完了させた状態を示します
        /// </summary>
        Done,
    }

    /// <summary>
    /// ワーカーの生存期間
    /// 基本的にSchedule関数<see cref="Schedule"/>しか使われない想定
    /// 開発進行中に必要があったらどんどん追加してください
    /// Note：Default ~ Foreverの間だけ追加可能
    /// </summary>
    public enum WorkerLifeTime
    {
        /// <summary>
        /// デフォルトのライフタイム
        /// </summary>
        Default = 0,

        /// <summary>
        /// 永遠に生きる（このSingletonは破壊されない限り）
        /// Note: MAX値のためこれ以上を追加しないこと
        /// </summary>
        Forever = 100
    }

    /// <summary>
    /// コルーチンを走らせる
    /// NOTE: tao-r Pause/Resume使用したい場合、コルーチン内にWaitForSecondsがないようご注意ください
    /// </summary>
    /// <param name="coroutine">コルーチン</param>
    /// <param name="lifeTime">生存期間</param>
    /// <returns>ワーカー</returns>
    public static IScheduleWorker RunCoroutine(IEnumerator coroutine, WorkerLifeTime lifeTime = WorkerLifeTime.Default)
    {
        var worker = new ScheduleWorker(coroutine, lifeTime);
        Instance.StartWorker(worker);
        return worker;
    }

    /// <summary>
    /// 1フレーム遅延して実行を行う
    /// </summary>
    /// <param name="action">遅延後の処理</param>
    /// <param name="lifeTime">生存期間</param>
    /// <returns>ワーカー</returns>
    public static IScheduleWorker DelayFrame(Action action, WorkerLifeTime lifeTime = WorkerLifeTime.Default)
    {
        return DelayFrame(1, action, lifeTime);
    }

    /// <summary>
    /// 任意フレーム遅延実行を行う
    /// </summary>
    /// <param name="count">遅延したいフレーム数</param>
    /// <param name="action">遅延後の処理</param>
    /// <param name="lifeTime">生存期間</param>
    /// <returns>ワーカー</returns>
    public static IScheduleWorker DelayFrame(int count, Action action, WorkerLifeTime lifeTime = WorkerLifeTime.Default)
    {
        var worker = new FrameScheduleWorker(count, false, lifeTime, action);
        Instance.StartWorker(worker);
        return worker;
    }

    /// <summary>
    /// 任意秒数遅延実行を行う
    /// </summary>
    /// <param name="seconds">遅延したい秒数</param>
    /// <param name="action">遅延後の処理</param>
    /// <param name="lifeTime">生存期間</param>
    /// <returns>ワーカー</returns>
    public static IScheduleWorker DelaySeconds(float seconds, Action action, WorkerLifeTime lifeTime = WorkerLifeTime.Default)
    {
        var worker = new TimeScheduleWorker(seconds, false, lifeTime, action);
        Instance.StartWorker(worker);
        return worker;
    }

    /// <summary>
    /// 指定した間隔で繰り返し実行を行う
    /// </summary>
    /// <param name="seconds">間隔秒数</param>
    /// <param name="action">間隔秒数経った後の処理</param>
    /// <param name="lifeTime">生存期間</param>
    /// <returns>ワーカー</returns>
    public static IScheduleWorker Schedule(float seconds, Action action, WorkerLifeTime lifeTime = WorkerLifeTime.Default)
    {
        var worker = new TimeScheduleWorker(seconds, true, lifeTime, action);
        Instance.StartWorker(worker);
        return worker;
    }

    /// <summary>
    /// 対象のワーカー停止する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    public static void Stop(IScheduleWorker worker)
    {
        Instance.StopWorker(worker);
    }

    /// <summary>
    /// 対象のワーカー停止して、設定された処理をすぐに行う
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    public static void StopWithAction(IScheduleWorker worker)
    {
        worker.Action?.Invoke();
        Instance.StopWorker(worker);
    }

    /// <summary>
    /// 指定生存期間とその生存期間以下のすべてワーカーを止める
    /// </summary>
    /// <param name="lifeTime">生存期間</param>
    public static void StopAll(WorkerLifeTime lifeTime)
    {
        Instance.StopAllWorkers(lifeTime);
    }

    /// <summary>
    /// 対象のワーカーを一時停止する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    public static void Pause(IScheduleWorker worker)
    {
        Instance.PauseWorker(worker);
    }

    /// <summary>
    /// 一時停止の対象ワーカーを復帰する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    public static void Resume(IScheduleWorker worker)
    {
        Instance.ResumeWorker(worker);
    }

    /// <summary>
    /// シングルトン生成させる
    /// </summary>
    public void EnsureCreation()
    {
    }

    /// <summary>
    /// シングルトン破壊時にすべてのスケジューラーを止める
    /// </summary>
    protected override void OnFinalize()
    {
        // OnFinalizeの中でInstanceを使っちゃダメ
        StopAllWorkers(WorkerLifeTime.Forever);
    }

    /// <summary>
    /// ワーカーを開始する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    void StartWorker(IScheduleWorker worker)
    {
        // コルーチンがすぐに終わる可能性があるため、
        // 先にリストに追加しないと、リストからの削除処理が先に実行されてしまう
        workerList.Add(worker);
        worker.Main = MainTask(worker).Flatten();
        StartCoroutine(worker.Main);
    }

    /// <summary>
    /// ワーカーを停止する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    void StopWorker(IScheduleWorker worker)
    {
        Complete(worker);
    }

    /// <summary>
    /// 指定生存期間とその生存期間以下のすべてワーカーを止める
    /// </summary>
    /// <param name="lifeTime">生存期間</param>
    void StopAllWorkers(WorkerLifeTime lifeTime)
    {
        foreach (var worker in new List<IScheduleWorker>(workerList))
        {
            if (worker.LifeTime > lifeTime)
            {
                continue;
            }

            Complete(worker);
        }
    }

    /// <summary>
    /// ワーカーを一時停止する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    void PauseWorker(IScheduleWorker worker)
    {
        if (worker.State == WorkerState.Working)
        {
            worker.State = WorkerState.Waiting;
            StopCoroutine(worker.Main);
        }
    }

    /// <summary>
    /// 一時停止のワーカーを再開する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    void ResumeWorker(IScheduleWorker worker)
    {
        if (worker.State == WorkerState.Waiting)
        {
            worker.State = WorkerState.Working;
            StartCoroutine(worker.Main);
        }
    }

    /// <summary>
    /// メインタスクを実行する
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    /// <returns>コルーチン</returns>
    IEnumerator MainTask(IScheduleWorker worker)
    {
        worker.State = WorkerState.Working;

        // worker.Mainだけ制御したいため、Main以外のコルーチをここでフラットに展開する
        while (worker.State == WorkerState.Working && worker.Iter.MoveNext())
        {
            yield return worker.Iter.Current;
        }

        Complete(worker);
    }

    /// <summary>
    /// ワーカー完了時に呼ばれる
    /// </summary>
    /// <param name="worker">対象ワーカー</param>
    void Complete(IScheduleWorker worker)
    {
        if (worker == null)
        {
            return;
        }

        // すでに完了している
        if (worker.State == WorkerState.Done)
        {
            if (workerList.Contains(worker))
            {
                Debug.LogError("Shouldn't happen. Worker should not be in managed list");
            }

            return;
        }

        worker.State = WorkerState.Done;

        if (worker.Main != null)
        {
            StopCoroutine(worker.Main);
            worker.Main = null;
        }

        // リストから消しておく
        if (!workerList.Remove(worker))
        {
            Debug.LogError("Shouldn't happen. Cannot find worker in managed list");
        }
    }

    /// <summary>
    /// 各種ワーカーの抽象インタフェース
    /// </summary>
    public interface IScheduleWorker
    {
        /// <summary>
        /// 遅延実行タスク
        /// </summary>
        IEnumerator Iter { get; }

        /// <summary>
        /// 遅延後処理
        /// </summary>
        Action Action { get; }

        /// <summary>
        /// 生存期間
        /// </summary>
        WorkerLifeTime LifeTime { get; }

        /// <summary>
        /// メインコルーチン
        /// </summary>
        IEnumerator Main { get; set; }

        /// <summary>
        /// ワーカーの状態
        /// </summary>
        WorkerState State { get; set; }
    }

    /// <summary>
    /// ワーカークラス
    /// </summary>
    public class ScheduleWorker : IScheduleWorker
    {
        /// <summary>
        /// 実行タスク
        /// </summary>
        public IEnumerator Iter { get; private set; }

        /// <summary>
        /// 実行アクション
        /// </summary>
        public Action Action { get; private set; }

        /// <summary>
        /// 生存期間
        /// </summary>
        public WorkerLifeTime LifeTime { get; private set; }

        /// <summary>
        /// メインコルーチン
        /// </summary>
        public IEnumerator Main { get; set; }

        /// <summary>
        /// ワーカーの状態
        /// </summary>
        public WorkerState State { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="iter">コルーチンタスク</param>
        /// <param name="lifetime">生存期間</param>
        public ScheduleWorker(IEnumerator iter, WorkerLifeTime lifetime)
        {
            Iter = iter;
            Action = null;
            LifeTime = lifetime;
            State = WorkerState.Waiting;
        }
    }

    /// <summary>
    /// ワーカークラス
    /// </summary>
    public class FrameScheduleWorker : IScheduleWorker
    {
        /// <summary>
        /// 遅延目標フレーム数
        /// </summary>
        public int TargetFrameCount { get; private set; }

        /// <summary>
        /// 無限にループするか
        /// </summary>
        public bool Loop { get; private set; }

        /// <summary>
        /// 実行タスク
        /// </summary>
        public IEnumerator Iter { get; private set; }

        /// <summary>
        /// 実行アクション
        /// </summary>
        public Action Action { get; private set; }

        /// <summary>
        /// 生存期間
        /// </summary>
        public WorkerLifeTime LifeTime { get; private set; }

        /// <summary>
        /// メインコルーチン
        /// </summary>
        public IEnumerator Main { get; set; }

        /// <summary>
        /// ワーカーの状態
        /// </summary>
        public WorkerState State { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="count">遅延フレーム数</param>
        /// <param name="loop">無限にループするか</param>
        /// <param name="lifetime">生存期間</param>
        /// <param name="action">コルーチン完了後のコールバック</param>
        public FrameScheduleWorker(int count, bool loop, WorkerLifeTime lifetime, Action action)
        {
            TargetFrameCount = count;
            Loop = loop;
            Iter = FrameIterator();
            Action = action;
            LifeTime = lifetime;
            State = WorkerState.Waiting;
        }

        /// <summary>
        /// フレーム遅延関数
        /// </summary>
        /// <returns>コルーチン</returns>
        IEnumerator FrameIterator()
        {
            do
            {
                for (var i = 0; i < TargetFrameCount; i++)
                {
                    yield return null;
                }

                Action?.Invoke();
            }
            while (Loop);
        }
    }

    /// <summary>
    /// ワーカークラス
    /// </summary>
    public class TimeScheduleWorker : IScheduleWorker
    {
        /// <summary>
        /// 1間隔内に経過した時間
        /// </summary>
        public float StackTime { get; private set; }

        /// <summary>
        /// 間隔目標時間
        /// </summary>
        public float TargetTime { get; private set; }

        /// <summary>
        /// 無限にループするかどうか
        /// </summary>
        public bool Loop { get; private set; }

        /// <summary>
        /// 実行タスク
        /// </summary>
        public IEnumerator Iter { get; private set; }

        /// <summary>
        /// 実行アクション
        /// </summary>
        public Action Action { get; private set; }

        /// <summary>
        /// 生存期間
        /// </summary>
        public WorkerLifeTime LifeTime { get; private set; }

        /// <summary>
        /// メインコルーチン
        /// </summary>
        public IEnumerator Main { get; set; }

        /// <summary>
        /// ワーカーの状態
        /// </summary>
        public WorkerState State { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="time">目標間隔時間</param>
        /// <param name="loop">無限にループするか</param>
        /// <param name="lifetime">生存期間</param>
        /// <param name="action">コルーチン完了後のコールバック</param>
        public TimeScheduleWorker(float time, bool loop, WorkerLifeTime lifetime, Action action)
        {
            StackTime = 0f;
            TargetTime = time;
            Loop = loop;
            Iter = TimeIterator();
            Action = action;
            LifeTime = lifetime;
            State = WorkerState.Waiting;
        }

        /// <summary>
        /// 時間遅延関数
        /// </summary>
        /// <returns>コルーチン</returns>
        IEnumerator TimeIterator()
        {
            do
            {
                StackTime = 0;

                while (StackTime < TargetTime)
                {
                    StackTime += Time.unscaledDeltaTime;
                    yield return null;
                }

                Action?.Invoke();
            }
            while (Loop);
        }
    }
}