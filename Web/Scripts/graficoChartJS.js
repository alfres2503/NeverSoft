////https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.2.2/Chart.bundle.min.js

function grafico(titulo, tituloEtiquetas, tipo, colores, etiquetas, valores) {
    var data =
    {
        labels: etiquetas,
        datasets: [{
            label: titulo,
            backgroundColor: colores,
            borderWidth: 2,
            data: valores
        }]
    };

    var ctx1 = document.getElementById("grafico").getContext("2d");
    window.myBar = new Chart(ctx1,
        {
            type: tipo,
            data: data,
            options:
            {
                animation: {
                    duration: 1000 //tiempo de animación general
                },

                hover: {
                    animationDuration: 3000 //duración de las animaciones al pasar el cursor sobre un elemento
                },
                responsiveAnimationDuration: 3000, //duración de la animación después de un cambio de tamaño
                legend: { display: true },
                title:
                {
                    display: true,
                    text: tituloEtiquetas
                },
                responsive: true,
                maintainAspectRatio: true,

            }
        });

}