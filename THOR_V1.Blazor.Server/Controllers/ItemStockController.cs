using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOR_V1.Module.BusinessObjects;

namespace THOR_V1.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class ItemStockController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public SimpleAction ScanBarcodeAction { get; }
        public ItemStockController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ItemStock);
            TargetViewType = ViewType.ListView;

            ScanBarcodeAction = new SimpleAction(
            this,
            "ScanBarcode",
            PredefinedCategory.View)
            {
                Caption = "📷 Scan Barcode"
            };

            ScanBarcodeAction.Execute += ScanBarcodeAction_Execute;
        }
        private async void ScanBarcodeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var js = (IJSRuntime)Application.ServiceProvider.GetService(typeof(IJSRuntime));
            if (js != null)
            {
                try
                {
                    var barcode = await js.InvokeAsync<string>("barcodeScanner.open");
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        // Gán barcode vào field bạn muốn
                        //stock.Barcode = barcode;
                        //ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage($"Scanned: {barcode}");
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage($"barcode: NULL");
                    }
                }
                catch (JSException ex)
                {
                    Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}");
                }
            }
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
