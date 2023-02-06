module.exports = class PriceChangeRequestModel {
    constructor(id, user_id, user_nickname, store_id, store_name, image_url, product_barcode, date_time, new_price, status) {
        this.id = id
        this.user_id = user_id
        this.user_nickname = user_nickname
        this.store_id = store_id
        this.store_name = store_name
        this.image_url = image_url
        this.product_barcode = product_barcode
        this.date_time = date_time
        this.new_price = new_price
        this.status = status //accepted, in progress, rejected
    }

}