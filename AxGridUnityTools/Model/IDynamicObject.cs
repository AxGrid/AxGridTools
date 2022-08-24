namespace AxGrid.Model
{
    public interface IDynamicObject
    {
        DynamicModel ModelLink { get; set; }
        string ModelField { get; set; }
        
    }
}