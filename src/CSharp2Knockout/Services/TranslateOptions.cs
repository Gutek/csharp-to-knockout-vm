using System;

namespace CSharp2Knockout.Services
{
    public class TranslateOptions
    {
        private bool? _publicOnly;
        public bool? PublicOnly
        {
            get { return _publicOnly.OrDefault(true); }
            set { _publicOnly = value; }
        }
        private bool? _includeEmptyType;
        public bool? IncludeEmptyType
        {
            get { return _includeEmptyType.OrDefault(false); }
            set { _includeEmptyType = value; }
        }

        private bool? _publicGetter;
        public bool? PublicGetter
        {
            get { return _publicGetter.OrDefault(true); }
            set { _publicGetter = value; }
        }

        private bool? _includeEnums;
        public bool? IncludeEnums
        {
            get { return _includeEnums.OrDefault(false); }
            set { _includeEnums = value; }
        }

        private bool? _includeDefaultData;
        public bool? IncludeDefaultData
        {
            get { return _includeDefaultData.OrDefault(false); }
            set { _includeDefaultData = value; }
        }
        private bool? _sortProps;
        public bool? SortProps
        {
            get { return _sortProps.OrDefault(true); }
            set { _sortProps = value; }
        }

        private bool? _camelCase;
        public bool? CamelCase
        {
            get
            {
                return _camelCase.OrDefault(false);
            }
            set { _camelCase = value; }
        }

        private bool? _forceCamelCase;
        public bool? ForceCamelCase
        {
            get
            {
                return _forceCamelCase.OrDefault(false);
            }
            set { _forceCamelCase = value; }
        }

        public static TranslateOptions Defaults { get { return new TranslateOptions(); } }

        public bool UseCamelCaseForKo
        {
            get { return CamelCase.IsTrue() || ForceCamelCase.IsTrue(); }
        }

        public bool UseCamelCaseForAll
        {
            get { return ForceCamelCase.IsTrue(); }
        }
    }
}