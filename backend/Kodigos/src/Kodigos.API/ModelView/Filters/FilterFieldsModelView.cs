namespace Kodigos.API.ModelView.Filters
{
    public class FilterFieldsModelView 
    {
        public string description { get; set; }
    }


    public class FilterFieldsStateCountModelView
    {
        public string state { get; set; }

        public List<string> counties { get; set; }
    }


}
