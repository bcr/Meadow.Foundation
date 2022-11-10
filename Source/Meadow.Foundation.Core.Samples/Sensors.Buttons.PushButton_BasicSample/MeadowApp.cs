﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Peripherals.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowApp
{
    /// <summary>
    /// This sample illustrates basic push button usage. To use, add a button that
    /// terminates on the `3V3` rail on one end, and `D02` on the other, such
    /// that when the button is pressed, `D02` is raised `HIGH`.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        PushButton pushButton;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);

            // intialize the push button
            pushButton = new PushButton(
                Device,
                Device.Pins.D02,
                Meadow.Hardware.ResistorMode.InternalPullDown);

            //---- wire up the Classic .NET events
            // `PressStarted`
            pushButton.PressStarted += (s, e) => {
                Console.WriteLine("pushButton.PressStarted.");
                onboardLed.SetColor(WildernessLabsColors.AzureBlue);
            };
            // `PressEnded`
            pushButton.PressEnded += (s, e) => {
                Console.WriteLine("pushButton.PressEnded.");
                onboardLed.IsOn = false;
            };
            // `Clicked`
            pushButton.Clicked += (s, e) => {
                Console.WriteLine("pushButton.Clicked.");
                onboardLed.SetColor(WildernessLabsColors.PearGreen);
                Thread.Sleep(250);
                onboardLed.IsOn = false;
            };
            // `LongPressClicked`
            pushButton.LongClicked += (s, e) => {
                Console.WriteLine("pushButton.LongClicked.");
                onboardLed.SetColor(WildernessLabsColors.ChileanFire);
                Thread.Sleep(1000);
                onboardLed.IsOn = false;
            };

            Console.WriteLine("Hardware initialized.");

            return Task.CompletedTask;
        }

    }
}