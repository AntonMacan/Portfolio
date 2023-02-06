$(document).on('click', '.deleterow', function () {
    event.preventDefault();
    var tr = $(this).parents("tr");
    tr.remove();
    clearOldMessage();
});

$(function () {
    $(".form-control").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
        }
    });

    $("#izlaz-cijenaIA, #izlaz-cijenaI, #izlaz-cijenaII, #izlaz-cijenaIII, #izlaz-cijenaIV").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
            dodajArtikl();
        }
    });


    $("#izlaz-dodaj").click(function () {
        event.preventDefault();
        dodajArtikl();
    });
});

function dodajArtikl() {
    var sifra = $("#izlaz-sifra").val();
    if (sifra != '') {
        if ($("[name='StavkeCjenika[" + sifra + "].IzlazID'").length > 0) {
            alert('Naplatna postaja je već jedan od izlaza');
            return;
        }

        var geoduzina = parseFloat($("#izlaz-geoduzina").val().replace(',', '.'));
        var geosirina = parseFloat($("#izlaz-geosirina").val().replace(',', '.'));

        var cijenaIA = parseFloat($("#izlaz-cijenaIA").val().replace(',', '.')); //treba biti točka, a ne zarez za parseFloat
        var cijenaI = parseFloat($("#izlaz-cijenaI").val().replace(',', '.'));
        var cijenaII = parseFloat($("#izlaz-cijenaII").val().replace(',', '.'));
        var cijenaIII = parseFloat($("#izlaz-cijenaIII").val().replace(',', '.'));
        var cijenaIV = parseFloat($("#izlaz-cijenaIV").val().replace(',', '.'));

        if (isNaN(cijenaIA)
            || isNaN(cijenaI)
            || isNaN(cijenaII)
            || isNaN(cijenaIII)
            || isNaN(cijenaIV)        ) {
            alert('Cijena mora biti broj!')
            return;
        }

        var cijenaIAE = cijenaIA / 7.53;
        var cijenaIE = cijenaI / 7.53;
        var cijenaIIE = cijenaII / 7.53;
        var cijenaIIIE = cijenaIII / 7.53;
        var cijenaIVE = cijenaIV / 7.53;

       
        var template = $('#template').html();
        var naziv = $("#izlaz-ime").val();
      
        var cijenaIA_formatirana = cijenaIA.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ' kn (€' + cijenaIAE.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ')'
        var cijenaI_formatirana = cijenaI.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ' kn (€' + cijenaIE.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ')'

        var cijenaII_formatirana = cijenaII.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ' kn (€' + cijenaIIE.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ')'
            
        var cijenaIII_formatirana = cijenaIII.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ' kn (€' + cijenaIIIE.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ')'
        var cijenaIV_formatirana = cijenaIV.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ' kn (€' + cijenaIVE.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + ')'


        //Alternativa ako su hr postavke sa zarezom //http://haacked.com/archive/2011/03/19/fixing-binding-to-decimals.aspx/
        //ili ovo http://intellitect.com/custom-model-binding-in-asp-net-core-1-0/

        template = template.replace(/--sifra--/g, sifra)
            .replace(/--ime--/g, naziv)
            .replace(/--cijenaIA--/g, cijenaIA)
            .replace(/--cijenaI--/g, cijenaI)
            .replace(/--cijenaII--/g, cijenaII)
            .replace(/--cijenaIII--/g, cijenaIII)
            .replace(/--cijenaIV--/g, cijenaIV)
            .replace(/--geoduzina--/g, geoduzina)
            .replace(/--geosirina--/g, geosirina)


            .replace(/--cijenaIA_formatirana--/g, cijenaIA_formatirana)
            .replace(/--cijenaI_formatirana--/g, cijenaI_formatirana)
            .replace(/--cijenaII_formatirana--/g, cijenaII_formatirana)
            .replace(/--cijenaIII_formatirana--/g, cijenaIII_formatirana)
            .replace(/--cijenaIV_formatirana--/g, cijenaIV_formatirana)

           
        $(template).find('tr').insertBefore($("#table-stavke").find('tr').last());

        $("#izlaz-sifra").val('');     
        $("#izlaz-cijenaIA").val('');
        $("#izlaz-cijenaI").val('');
        $("#izlaz-cijenaII").val('');
        $("#izlaz-cijenaIII").val('');
        $("#izlaz-cijenaIV").val('');
        $("#izlaz-geoduzina").val('');
        $("#izlaz-geosirina").val('');

        $("#izlaz-naziv").val('');

        clearOldMessage();
    }
}