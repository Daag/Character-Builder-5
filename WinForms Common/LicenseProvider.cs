﻿using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Forms
{
    public class LicenseProvider : ILicense
    {
        public bool ShowLicense(string title, string[] lines)
        {
            System.Windows.Forms.DialogResult r = new License(title, lines).ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
