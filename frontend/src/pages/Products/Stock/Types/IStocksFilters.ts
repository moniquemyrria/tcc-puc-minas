import {IStocksGroups }  from "./IStocksGroups"
import {IStocksCategory }  from "./IStocksCategory"

export class IStocksFilters {
    code: string = ""
    filial: string = ""
    description: string = ""
    local: string = ""
    group: IStocksGroups = new IStocksGroups
    category: IStocksCategory = new IStocksCategory
}

export class IStocksContractFilters {
    fieldName: string = ""
    filterData: string = ""
}
