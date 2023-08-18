import { IStocksCategory } from "./IStocksCategory"
import { IStocksGroups } from "./IStocksGroups"

export class IStocks {
    code: string = ""
    filial: string = ""
    description: string = ""
    local: string = ""
    balance: number = 0 
    empenho: number = 0 
    balanceAvailable: number = 0 
    group: IStocksGroups = new IStocksGroups
    category: IStocksCategory = new IStocksCategory
}