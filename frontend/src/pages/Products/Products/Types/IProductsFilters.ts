import { IProductsGroups } from "./IProductsGroups"
import { IProductsTypes } from "./IProductsTypes"

export class IProductsFilters {
    code: string = ""
    description: string = ""
    status: string = ""
    group: IProductsGroups = new IProductsGroups
    type: IProductsTypes = new IProductsTypes
}


export class IProductsContractFilters {
    fieldName: string = ""
    filterData: string = ""
}



