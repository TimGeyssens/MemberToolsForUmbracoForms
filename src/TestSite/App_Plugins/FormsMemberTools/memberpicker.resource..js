function memberPickerResource($http) {

    var apiRoot = "backoffice/FormsMemberTools/Member/";

    return {
 
        getAllMemberTypesWithAlias: function () {
            return $http.get(apiRoot + "GetAllMemberTypesWithAlias");
        },
        getAllProperties: function (membertypeAlias) {
            return $http.get(apiRoot + "GetAllProperties?membertypeAlias=" + doctypeAlias);
        }

    };
}

angular.module('umbraco.resources').factory('memberPickerResource', pickerResource);