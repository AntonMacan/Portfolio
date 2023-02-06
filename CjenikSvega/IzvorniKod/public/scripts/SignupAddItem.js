showBasic = function() {
    if(document.getElementById("chkYes").checked){
        document.getElementById("trgovina").style.display = 'block'
    }
    else {
        document.getElementById("trgovina").style.display = 'none'
    }

    return;
}

addItem = function() {
    let btns = document.getElementById("btns");
    btns.remove();

    let unos = document.createElement("div");
    unos.style.display = "flex";
    unos.style.justifyContent = "space-between";

    let kod = document.createElement("input");
    kod.type = "text";
    kod.name = "kod";

    unos.appendChild(kod);
    
    let ime = document.createElement("input");
    ime.type = "text";
    ime.name = "ime";

    unos.appendChild(ime);
    
    let cijena = document.createElement("input");
    cijena.type = "text";
    cijena.name = "cijena";

    unos.appendChild(cijena);

    let trgovina = document.getElementById("trgovina")
    trgovina.appendChild(unos);
    
    btns = document.createElement("div");
    btns.id = "btns";

    let btnAdd = document.createElement("button");
    btnAdd.type = "button";
    btnAdd.addEventListener("click", addItem)
    btnAdd.textContent = "Dodaj proizvod";

    btns.appendChild(btnAdd);

    trgovina.appendChild(btns)
    return;            
}

removeItem = function() {

}
