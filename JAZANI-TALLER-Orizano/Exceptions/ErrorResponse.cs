namespace JAZANI_TALLER_Orizano.Exceptions
{
    public class ErrorResponse
    {
        public string? Message { get; set; }
        public IList<ErrorValidationModel> Errors { get; set; }
    }
}
