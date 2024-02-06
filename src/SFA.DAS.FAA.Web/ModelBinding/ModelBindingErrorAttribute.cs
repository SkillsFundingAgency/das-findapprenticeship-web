namespace SFA.DAS.FAA.Web.ModelBinding
{
    public class ModelBindingErrorAttribute(string errorMessage) : Attribute
    {
        public string ErrorMessage { get;  } = errorMessage;
    }
}
