//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace hotel_version1._0
{
    using System;
    using System.Collections.Generic;
    
    public partial class HOTEL_ITEM_USED
    {
        public string ITEM_TYPE { get; set; }
        public string ORDER_ID { get; set; }
        public decimal NUM { get; set; }
    
        public virtual HOTEL_ITEM HOTEL_ITEM { get; set; }
        public virtual HOTEL_ORDER HOTEL_ORDER { get; set; }
    }
}
