module.exports = class NotificationModel {
    constructor(id, dateTime, text, seen) {
        this.id = id
        this.dateTime = dateTime
        this.text = text
        this.seen = seen
    }
}