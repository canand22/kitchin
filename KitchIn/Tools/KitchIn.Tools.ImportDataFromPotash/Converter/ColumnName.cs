using System;

namespace KitchIn.Tools.ImportDataFromPotash.Converter
{
    class ColumnName
    {
        /// <summary>
        /// название колонки, для загружаемых данных
        /// </summary>
        public String Name { get; private set; }
        /// <summary>
        /// буква колонки
        /// </summary>
        public String Liter { get; private set; }

        public ColumnName(string name, string liter)
        {
            Name = name;
            Liter = liter;
        }
    }
}
