﻿using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ItemChoiceFeatureForm : Form, IEditor<ItemChoiceFeature>
    {
        private ItemChoiceFeature bf;
        public ItemChoiceFeatureForm(ItemChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            CP.DataBindings.Add("Value", f, "CP", true, DataSourceUpdateMode.OnPropertyChanged);
            SP.DataBindings.Add("Value", f, "SP", true, DataSourceUpdateMode.OnPropertyChanged);
            GP.DataBindings.Add("Value", f, "GP", true, DataSourceUpdateMode.OnPropertyChanged);
            basicFeature1.Feature = f;
            stringList1.Items = f.Items;
            Program.Context.ImportItems();
            stringList1.Suggestions = Program.Context.ItemsSimple.Keys;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ItemChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

