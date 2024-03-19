using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.Utility
{
    public static class SD
    {
        public const string Role_User_Cust = "Customer";
        public const string Role_User_Comp = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";


        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentSatusPending = "Pending";
        public const string PaymentSatusApproved = "Approved";
        public const string PaymentSatusDelayPayment = "ApprovedForDelayPayment";
        public const string PaymentSatusRejected = "Rejected";



    }
}
