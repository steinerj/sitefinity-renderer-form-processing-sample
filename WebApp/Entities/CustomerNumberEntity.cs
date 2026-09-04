using Progress.Sitefinity.AspNetCore.FormWidgets.Entities.TextField;

public sealed class CustomerNumberEntity : TextFieldEntity
{
    public CustomerNumberEntity()
    {
        this.Label = "Customer number";
        this.PlaceholderText = "CUST-1000";
    }
}