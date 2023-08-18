import { IResult } from "../types/IKResult";
import axios from "../../modules/axios";

export default class Api {

    /**
     * 
     * @param url 
     * @description Axios Get 
     * @returns Promise<IKResult<T>>
     */
    static async getResult<T>(url: string,): Promise<IResult> {

        let result = {} as IResult

        await axios.get(url).then((r) => {

            result = r

        }).catch((e) => {
            result.data.sucesso = false
            result.data.errors = [{ code: '999', description: e }]
        })

        return result

    }

    static async getIdResult<T>(url: string, id: string): Promise<IResult> {

        let result = {} as IResult

        await axios.get(url + "/" + id).then((r) => {
            
            result = r

        }).catch((e) => {
            result.data.sucesso = false
            result.data.errors = [{ code: '999', description: e }]
        })

        return result

    }

    static async postResult<T>(url: string, obj: any): Promise<IResult> {

        let result = {} as IResult

        await axios.post(url, obj).then((r) => {

            result = r

        }).catch((e) => {
            result.data.sucesso = false
            result.data.errors = [{ code: '999', description: e }]
        })

        return result
    }

    static async putResult<T>(url: string, obj: any): Promise<IResult> {

        let result = {} as IResult

        await axios.put(url + "/" + obj.id, obj).then((r) => {

            result = r

        }).catch((e) => {
            result.data.sucesso = false
            result.data.errors = [{ code: '999', description: e }]
        })

        return result
    }

    static async deleteResult<T>(url: string, id: any): Promise<IResult> {

        let result = {} as IResult

        await axios.delete(url + "/" + id).then((r) => {

            result = r

        }).catch((e) => {
            result.data.sucesso = false
            result.data.errors = [{ code: '999', description: e }]
        })

        return result
    }

    static async putRefreshPasswordResult<T>(url: string, obj: any): Promise<IResult> {

        let result = {} as IResult

        await axios.put(url, obj).then((r) => {

            result = r

        }).catch((e) => {
            result.data.sucesso = false
            result.data.errors = [{ code: '999', description: e }]
        })

        return result
    }

}