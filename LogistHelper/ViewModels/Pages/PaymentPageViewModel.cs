using CommunityToolkit.Mvvm.Input;
using DTOs;
using LogistHelper.Services;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Pages
{
    public class PaymentPageViewModel : BasePageViewModel
    {
        #region Private

        private IDataAccess _access;
        private IMessageDialog _dialog;

        private bool _isBlock;
        private string _blockText = "Печать";

        private List<RequredToPayContractViewModel> _requiredToPay;

        private string _printString;
        #endregion Private

        #region Public

        public bool IsBlock 
        { 
            get => _isBlock;
            set => SetProperty(ref _isBlock, value);
        }

        public string BlockText
        {
            get => _blockText;
            set => SetProperty(ref _blockText, value);
        }

        public List<RequredToPayContractViewModel> RequiredToPay 
        {
            get => _requiredToPay;
            set => SetProperty(ref _requiredToPay, value);
        }

        #endregion Public

        #region Commands

        public ICommand PrintPaymentsCommand { get; }

        #endregion Commands

        public PaymentPageViewModel(IDataAccess access, IMessageDialog dialog)
        {
            _access = access;
            _dialog = dialog;

            Load();

            #region CommandInit

            PrintPaymentsCommand = new RelayCommand(async () => 
            {
                IEnumerable<RequredToPayContractViewModel> toPrint = RequiredToPay.Where(r => r.IsSelectedToPrint);
                if (toPrint.Any())
                {
                    IsBlock = true;

                    _printString = string.Empty;

                    foreach (var item in toPrint)
                    {
                        _printString += $"{item.Number} от {item.CreationDate}, {item.Carrier}, {item.Balance.ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))}\n\n";
                    }


                    PrintDocument printDocument = new PrintDocument();

                    printDocument.PrintPage += PrintPageHandler;
                    PrintDialog printDialog = new PrintDialog();

                    if (printDialog.ShowDialog() == true)
                    {
                        printDocument.Print();
                    }
                    printDocument.PrintPage -= PrintPageHandler;

                }
                else
                {
                    _dialog.ShowError("Выберите оплаты для печати");
                }

                IsBlock = false;
            });
            #endregion CommandInit
        }

        private async void Load() 
        {
            await Task.Run(async () => 
            {
                var loadResult = await _access.GetRequiredPayments<RequiredToPayContractDto>();

                if (loadResult.IsSuccess) 
                {
                    int count = 0;
                    RequiredToPay = loadResult.Result.Select(x => new RequredToPayContractViewModel(x, count++)).ToList();
                }
            });
        }

        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(_printString, new Font("Arial", 14), Brushes.Black, 50, 30);
        }

        protected override void BackCommandExecutor()
        {
            NavigationService.Navigate(Models.PageType.MainMenu);
        }
    }
}
