using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InvokeAzureFunction.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace InvokeAzureFunction.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ImageColors imageColors = null;
        private HttpClient client = new HttpClient();

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                base.RaisePropertyChanged();
            }
        }

        private string input;
        public string Input
        {
            get { return input; }
            set
            {
                input = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> results;
        public ObservableCollection<string> Results
        {
            get { return results; }
            set
            {
                results = value;
                base.RaisePropertyChanged();
            }
        }

        private bool wait;
        public bool Wait
        {
            get { return wait; }
            set
            {
                wait = value;
                base.RaisePropertyChanged();
            }
        }

        private string imageFile;
        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value;
                base.RaisePropertyChanged();
            }
        }

        public double RedPercentage
        {
            get
            {
                return (imageColors != null) ? imageColors.RedPercentage : 0;
            }
        }

        public double GreenPercentage
        {
            get
            {
                return (imageColors != null) ? imageColors.GreenPercentage : 0;
            }
        }

        public double BluePercentage
        {
            get
            {
                return (imageColors != null) ? imageColors.BluePercentage : 0;
            }
        }

        public RelayCommand CallAzureFunction1Command { get; set; }
        public RelayCommand CallAzureFunction2Command { get; set; }

        public MainViewModel()
        {
            this.CallAzureFunction1Command = new RelayCommand(callAzureFunction1CommandExecute);
            this.CallAzureFunction2Command = new RelayCommand(callAzureFunction2CommandExecute);

            this.Results = new ObservableCollection<string>();
            this.Url = "https://visualstudiotips.azurewebsites.net/api/GetSizeForUrl";
            this.Input = "http://www.visualstudiotips.net";
        }

        private async void callAzureFunction2CommandExecute()
        {
            Windows.Storage.Pickers.FileOpenPicker picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.CommitButtonText = "OK";
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                this.Wait = true;
                this.ImageFile = file.Path;
                var buffer = await FileIO.ReadBufferAsync(file);
                byte[] bytes = buffer.ToArray();

                string code = "f3RuUGE2BM8UjjA0XJ8g6br/eDP9L3NsG1n3MR8ZYfbipRiHQ0XiJg==";
                string url = "https://visualstudiotips.azurewebsites.net/api/GetImageColors?code=" + code;
                var response = await client.PostAsync(url, new ByteArrayContent(bytes));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsAsync<string>();
                    imageColors = JsonConvert.DeserializeObject<ImageColors>(json);
                    base.RaisePropertyChanged(nameof(RedPercentage));
                    base.RaisePropertyChanged(nameof(GreenPercentage));
                    base.RaisePropertyChanged(nameof(BluePercentage));
                }

                this.Wait = false;
            }
        }

        private async void callAzureFunction1CommandExecute()
        {
            this.Wait = true;

            HttpResponseMessage response;
            var input = new Input() { url = this.Input };
            response = await client.PostAsJsonAsync<Input>(this.Url, input);

            if (response.IsSuccessStatusCode)
            {
                this.Results.Add(await response.Content.ReadAsStringAsync());
            }
            else
            {
                this.Results.Add("#err#");
            }

            this.Wait = false;
        }
    }
}