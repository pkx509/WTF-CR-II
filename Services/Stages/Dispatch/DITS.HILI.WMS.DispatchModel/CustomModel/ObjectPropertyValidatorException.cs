namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class ObjectPropertyValidatorException
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string Message { get; set; }
        public string Property { get; set; }
        public ObjectPropertyValidatorException()
        {
            //
        }
        public ObjectPropertyValidatorException(int rowIndex, int colIndex, string message, string property)
        {
            RowIndex = rowIndex;
            ColumnIndex = colIndex;
            Property = property;
            Message = message;
        }
    }
}
