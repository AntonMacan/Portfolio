checkFile = async(file) =>{
   if(!(file instanceof File)|| file == null) return false;
   return true;
}

module.exports = {
   checkFile
}