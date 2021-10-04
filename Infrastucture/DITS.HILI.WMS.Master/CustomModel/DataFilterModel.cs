namespace DITS.HILI.WMS.MasterModel.Products
{
    public enum ConditionWhere { AND, OR, NONE }
    public class DataFilterModel
    {
        private string _op;
        public string Op
        {
            get
            {
                if (_op == "+")
                {
                    _op = "=";
                }

                return _op;
            }
            set => _op = value;
        }
        public string Type { get; set; }
        public string DataIndex { get; set; }
        public string Value { get; set; }
        public int Group { get; set; }

        private ConditionWhere _condition;
        public ConditionWhere Condition
        {

            get
            {
                if (string.IsNullOrEmpty(_condition.ToString()))
                {
                    _condition = ConditionWhere.NONE;
                }

                return _condition;
            }
            set => _condition = value;
        }

    }

}
