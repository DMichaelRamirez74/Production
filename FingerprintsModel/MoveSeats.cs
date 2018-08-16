using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class MoveSeats
    {


        private int _requestedPage = 1;
        private int _skip = 0;
        private int _take = 10;
        private string _searchText = string.Empty;

        public Guid AgencyID { get; set; }

        public int TotalPurchaedSeats { get; set; }

        public List<Center> CenterList { get; set; }

        public string SearchTerm { get { return _searchText; } set { _searchText = value; } }

        public List<SelectListItem> AgencyCenterList { get; set; }



        public List<CenteClassPair> CenterClassPairList { get; set; }

        public class CenteClassPair
        {
            public string FromCenter { get; set; }

            public string FromClassRoom { get; set; }
            public string ToCenter { get; set; }
            public string ToClassRoom { get; set; }

            public string SeatsMoved { get; set; }
        }

        public int Take
        {
            get
            {

                return _take;
            }
            set
            {
                _take = value;
            }
        }
        public int Skip { get { return _skip; } set { _skip = value; } }
        public string SearchText { get { return _searchText; } set { _searchText = value; } }

        public int RequestedPage { get { return _requestedPage; } set { _requestedPage = value; } }

        public int TotalRecord { get; set; }

        public string ProgramYear { get; set; }

        public bool IsEndOfYear { get; set; }


    }


}
