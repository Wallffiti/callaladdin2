﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallAladdin.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserProfileUserControl : ScrollView //Grid
    {
		public UserProfileUserControl ()
		{
			InitializeComponent ();
		}
	}
}