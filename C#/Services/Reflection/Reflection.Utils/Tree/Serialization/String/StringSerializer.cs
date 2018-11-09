using Reflection.Utils.Tree.Localization;
using System;

namespace Reflection.Utils.PropertyTree.Selialization {

    public static class StringSerializer {
        public static SerializeItem Serialize(PropertyField propertyField) {
            return new SerializeItem();
        }

        #region PropertyField
        //public override string ToString() {
        //    string format = LocalizationTable.GetStringById(LocalizationId.Name) + " = {0}, " + LocalizationTable.GetStringById(LocalizationId.Type) + " = {1}";
        //    return String.Format(format, IdToString(), TypeToString());
        //}

        //public string IdToString() {
        //    if (this.id == null)
        //        return LocalizationTable.GetStringById(LocalizationId.Null);
        //    if (this.id is string) {
        //        string stringId = (string)this.id;
        //        if (String.IsNullOrEmpty(stringId))
        //            return LocalizationTable.GetStringById(LocalizationId.Empty);
        //        return stringId;
        //    }
        //    return this.id.ToString();
        //}

        //public string TypeToString() {
        //    if (this.type == null)
        //        return LocalizationTable.GetStringById(LocalizationId.Null);
        //    Type underlyingType = Nullable.GetUnderlyingType(this.type);
        //    if (underlyingType != null)
        //        return LocalizationTable.GetStringById(LocalizationId.Nullable) + " " + underlyingType.Name;
        //    return this.type.Name;
        //} 
        #endregion

        #region TreeItem
        //public override string ToString() {
        //    return Value.ToString() + " " + GetChildrenStringInfo();
        //}

        //string GetChildrenStringInfo() {
        //    if (Children == null) {
        //        if (HasChildrenCycle)
        //            return
        //                "(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": " +
        //                LocalizationTable.GetStringById(LocalizationId.Null) + ", " +
        //                LocalizationTable.GetStringById(LocalizationId.HasObjectChildrenCycle) + ")";
        //        else
        //            return
        //                "(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": " +
        //                LocalizationTable.GetStringById(LocalizationId.Null) + ")";
        //    }
        //    int childrenCount = Children.Count();
        //    if (childrenCount == 0)
        //        return
        //            "(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": " +
        //            LocalizationTable.GetStringById(LocalizationId.Empty) + ")";
        //    return String.Format("(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": {0})", childrenCount);
        //}
        #endregion

        #region PropertyObjectChildren
        //public override string ToString() {
        //    string result = String.Format(LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": {0}", this.items.Count());
        //    if (this.hasCycle)
        //        result += ", " + LocalizationTable.GetStringById(LocalizationId.HasObjectChildrenCycle);
        //    return result;
        //}
        #endregion

        #region PropertyValueStringBuilder
        //public static SerializeItem Serialize(object propertyValue) { 
        //    //if (propertyValue == null)
        //    //    return LocalizationTable.GetStringById(LocalizationId.Value) + " = " + LocalizationTable.GetStringById(LocalizationId.Null);
        //    //string format = LocalizationTable.GetStringById(LocalizationId.Type) + " = {0}, " + LocalizationTable.GetStringById(LocalizationId.Value) + " = {1}";
        //    //Type type = propertyValue.GetType();
        //    //Type underlyingType = Nullable.GetUnderlyingType(type);
        //    //if (underlyingType != null)
        //    //    type = underlyingType;

        //    //return String.Format(format, type.Name, ValueToString(propertyValue, type));
        //}



        //static string ValueToString(object value, Type type) {
        //    //if (value is Exception)
        //    //    return LocalizationTable.GetStringById(LocalizationId.Exception);
        //    //TypeCode typeCode = Type.GetTypeCode(type);
        //    //if (typeCode == TypeCode.DBNull || typeCode == TypeCode.Empty)
        //    //    return LocalizationTable.GetStringById(LocalizationId.Null);
        //    //if (typeCode == TypeCode.String) {
        //    //    string stringValue = (string)value;
        //    //    return String.IsNullOrEmpty(stringValue) ? LocalizationTable.GetStringById(LocalizationId.Empty) : stringValue;
        //    //}
        //    //return value.ToString();
        //}
        #endregion
    }
}
