import CryptoJS from 'crypto-js';
export default class Security {
    private static key: string = "ks_0#$lkj"
    static encrypted(val: string) {
        var encrypted = CryptoJS.AES.encrypt(val, Security.key);
        
        return encrypted.toString();
    }
    static decrypted(encrypted: string) {
        var decrypted = CryptoJS.AES.decrypt(encrypted, Security.key);
        
        
        return decrypted.toString(CryptoJS.enc.Utf8)
    }
}