const ItemDataAccess = require('../data_access/ItemDataAccess.js');

module.exports = class Item {
	
	async loadItem(barcode) {
		this.name = await ItemDataAccess.getItemName(barcode);
		if (this.name != undefined)
			this.name = this.name.charAt(0).toUpperCase() + this.name.slice(1);
		
		this.stores = await ItemDataAccess.getStores(barcode);
		this.tags = await ItemDataAccess.getTags(barcode);
		
		for (let store of this.stores)
		{
			for (let price of store.prices)
			{
				let str;
				str = price.datetime.getDate();
				str = str + '.' + (price.datetime.getMonth() + 1) + '.' + price.datetime.getFullYear() + '. ';
				str = str + price.datetime.getHours() + ':' + price.datetime.getMinutes();
				
				price.datetime = str;
			}
		}
	}
}