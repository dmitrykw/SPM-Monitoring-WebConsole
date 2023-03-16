namespace SPM_WebConsole.Models.ViewModels.Options
{
    public class OptionsViewModel
    {
        public Settings Settings = new();


        public bool IsReadOnly { get { return App_Globals.IsReadOnly; } }


        public bool ApiIsAvailable { get; set; }
        public string ConnectionErrorHeader = "";


        public OptionsViewModel()
        {
            FillSettings();
        }


        private void FillSettings()
        {
            var spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);

            try
            {
                Settings = spm_api_processor.GetSettings();
                ApiIsAvailable = true;
            }
            catch (Exception ex)
            {
                ApiIsAvailable = false;
                ConnectionErrorHeader = App_Globals.ApiConnectioErrorText + " " + ex.Message;
            }


        }



        public void SendOptionsUpdateAPI(UpdateSettingsObj input_data)
        {
            if (IsReadOnly) { return; }

            var spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            { spm_api_processor.SendSettingsUpdate(input_data); }
            catch { throw; }
        }

    }
}
