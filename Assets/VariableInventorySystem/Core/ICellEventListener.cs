namespace VariableInventorySystem
{
    public interface ICellEventListener
    {
        void OnCellClick(ICell cell);
        void OnCellOptionClick(ICell cell);
        void OnCellEnter(ICell cell);
        void OnCellExit(ICell cell);
    }
}
