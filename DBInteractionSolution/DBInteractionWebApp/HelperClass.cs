namespace DBInteractionWebApp
{
    public static class HelperClass
    {
        public static Exception GetInnerException(Exception ex)
        {
            while(ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}
