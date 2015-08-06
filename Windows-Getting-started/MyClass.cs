/**
 * Created by Handpoint.
 * Copyright (c) 2015 Handpoint. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using com.handpoint.api;

namespace WindowsGettingStarted
{
    // MyClass implements the Events.Required and Events.Status so we can capture transaction and status notification events
    class MyClass : Events.Required, Events.Status
    {
        Hapi api;
        private MainWindow Window;
 
        public MyClass(MainWindow mainWindow1)
        {
            InitApi();
            Window = mainWindow1;
        }
 
        public void InitApi()
        {
            // If using a Handpoint integration card reader update the following string with the supplied shared secret
            string sharedSecret = "0102030405060708091011121314151617181920212223242526272829303132";
            api = HapiFactory.GetAsyncInterface(this).DefaultSharedSecret(sharedSecret);
            // The api is now initialized. Yay! we've even set a default shared secret!
            // The shared secret is a unique string shared between the card reader and your mobile application. 
            // It prevents other people to connect to your card reader.

            // Subscribe to the status notifications
            this.api.AddStatusNotificationEventHandler(this);
        }
  
        public void DeviceDiscoveryFinished(List<Device> devices)
        {
          // Only needed when using a payment terminal, here you get a list of Bluetooth payment terminals paired with your computer
          // You can also get a list of serial / USB payment terminals attached to your computer
         
        }
 
        public void Connect()
        {
            Device device = new Device("Name", "Address", "Port", ConnectionMethod.SIMULATOR); //This sample uses the built in card reader simulator
            api.UseDevice(device);
        }
     
        // Financial transaction functions
        public bool PayWithSignatureAuthorized()
        {
            return api.Sale(new BigInteger("10000"), Currency.GBP);
            // amount X00XX where X represents an integer [0;9] --> Signature authorized
        }
         
        public bool PayWithSignatureDeclined()
        {
            return api.Sale(new BigInteger("10100"), Currency.GBP);
            // amount X01XX where X represents an integer [0;9] --> Signature declined
        }
         
        public bool PayWithPinAuthorized()
        {
            return api.Sale(new BigInteger("11000"), Currency.GBP);
            // amount X10XX where X represents an integer [0;9] --> Pin authorized
        }
        public bool PayWithPinDeclined()
        {
            return api.Sale(new BigInteger("11100"), Currency.GBP);
            // amount X11XX where X represents an integer [0;9] --> Pin declined
        }
  
        public void SignatureRequired(SignatureRequest signatureRequest, Device device)
        {
            // You'll be notified here if a sale process needs a signature verification
            // A signature verification is only needed if the cardholder uses a magnetic stripe card or a chip and signature card for the payment
            // This method will not be invoked if a transaction is made with a Chip & Pin card
 
 
            api.SignatureResult(true); // This line means that the cardholder ALWAYS accepts to sign the receipt.
            // A specific line will be displayed on the merchant receipt for the cardholder to be able to sign it
        }
 
        // EndOfTransaction will be called at the end of a transaction
        public void EndOfTransaction(TransactionResult transactionResult, Device device)
        {
            Window.DisplayReceipts(transactionResult.MerchantReceipt, transactionResult.CustomerReceipt); 
        }

        // ConnectionStatusChanged will get called if the connections status changes
        public void ConnectionStatusChanged(ConnectionStatus connectionStatus, Device device)
        {
            String status = device.Name + " is " + connectionStatus.ToString();
            Debug.WriteLine(status);
            Window.DisplayStatus(status);
        }

        // CurrentTransactionStatus notifies about changes to the current transaction status
        public void CurrentTransactionStatus(StatusInfo statusInfo, Device device)
        {
            String status = statusInfo.Message;
            Debug.WriteLine(status);
            Window.DisplayStatus(status);
        }
 
    }
}
