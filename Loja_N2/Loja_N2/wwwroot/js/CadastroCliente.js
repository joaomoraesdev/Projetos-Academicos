function buscaCEP() {
    var cep = document.getElementById("Cep").value;
    cep = cep.replace('-', ''); // removemos o traço do CEP
    if (cep.length > 0) {
        var linkAPI = 'https://viacep.com.br/ws/' + cep + '/json/';
        $.ajax({
            url: linkAPI,
            beforeSend: function () {
                document.getElementById("Logradouro").value = '...';
                document.getElementById("Localidade").value = '...';
                document.getElementById("Uf").value = '...';
            },
            success: function (dados) {
                if (dados.erro != undefined) // quando o CEP não existe...
                {
                    alert('CEP não localizado...');
                    document.getElementById("Logradouro").value = '';
                    document.getElementById("Localidade").value = '';
                    document.getElementById("Uf").value = '';
                }
                else // quando o CEP existe
                {
                    document.getElementById("Logradouro").value = dados.logradouro;
                    document.getElementById("Localidade").value = dados.localidade;
                    document.getElementById("Uf").value = dados.uf;
                }
            }
        });
    }
}