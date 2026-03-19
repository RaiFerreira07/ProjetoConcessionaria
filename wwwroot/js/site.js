    $(document).ready(function () {
        // ===============================
        // CHAT
        // ===============================
        async function enviarPergunta() {
            let pergunta = $("#input").val().trim();
            if (!pergunta) return;

            $("#naosei").append(`
                <div class="mensagem usuario">
                    ${pergunta}
                    <span class="hora">${new Date().toLocaleTimeString()}</span>
                </div>
            `);

            $("#input").val("");
            // Animacao digitando do chat
            let digitando = $(`
                <div class="mensagem bot digitando">
                    <span></span><span></span><span></span>
                </div>
            `);
            $("#naosei").append(digitando);

            let resposta = await $.post("/Chat/EnviarPergunta", { pergunta: pergunta });

            digitando.remove();

            $("#naosei").append(`
                <div class="mensagem bot">
                    ${resposta}
                    <span class="hora">${new Date().toLocaleTimeString()}</span>
                </div>
            `);
            //scrolar a tela de acprdo com os textos
            $("#naosei").scrollTop($("#naosei")[0].scrollHeight);
        }

        $("#texto button").click(enviarPergunta);
        //Captura de teclas
        $(document).keydown(function (e) {
            if (e.which === 13) { // Enter
                e.preventDefault();
                enviarPergunta();
            }   
            if (e.which === 32) { // Espaço
                e.preventDefault();
                $("#input").focus();
            }
        });

        // ===============================
        // MODAIS AJAX
        // ===============================

        window.abrirDetalhes = function (controller, id) {
            $("#modalConteudo").load(`/${controller}/Details/${id}`, function (response, status) {
                if (status === "error") {
                    alert("Erro ao carregar detalhes: " + response);
                } else {
                    $("#modalAjax").modal("show");
                }
            });
        };

        window.abrirEditar = function (controller, id) {
            $("#modalConteudo").load(`/${controller}/Edit/${id}`, function (response, status) {
                if (status === "error") {
                    alert("Erro ao carregar edição: " + response);
                } else {
                    $("#modalAjax").modal("show");

                    // intercepta submit do form dentro do modal
                    $("#modalConteudo form").on("submit", function (e) {
                        e.preventDefault();
                        $.ajax({
                            type: "POST",
                            url: this.action,
                            data: $(this).serialize(),
                            success: function (res) {
                                if (res.success) {
                                    $("#modalAjax").modal("hide");
                                    location.reload();
                                } else {
                                    $("#modalConteudo").html(res);
                                }
                            }
                        });
                    });
                }
            });
        };
   
       //Adiciona o valor de um veiculo ja existente no form de criar venda     
        $("#VeiculoId").change(function () {
             var veiculoId = $(this).val();
             if (!veiculoId) {
                 $("#ValorFinal").val("");
                 return;
             }

             $.get("/Veiculos/GetPreco/" + veiculoId, function (data) {
                   // Preenche o input ValorFinal com o preço do veículo
                  $("#ValorFinal").val(data.preco);
             });
        });



        // ===============================
        // CSS dinamicamente injetado
        // ===============================
        $("<style>").prop("type", "text/css").html(`
            .mensagem {
                max-width: 60%;
                padding: 10px 15px;
                margin: 5px 0;
                border-radius: 15px;
                word-wrap: break-word;
                font-size: 14px;
                display: flex;
                flex-direction: column;
            }
            .usuario {
                align-self: flex-end;
                background-color: #1F598C;
                color: white;
                border-bottom-right-radius: 5px;
            }
            .bot {
                align-self: flex-start;
                background-color: #595959;
                color: white;
                border-bottom-left-radius: 5px;
            }
            .mensagem .hora {
                font-size: 10px;
                opacity: 0.6;
                margin-top: 3px;
                align-self: flex-end;
            }
            .digitando span {
                display: inline-block;
                width: 8px;
                height: 8px;
                margin: 0 2px;
                background-color: #ffffff;
                border-radius: 50%;
                animation: piscar 1s infinite;
            }
            .digitando span:nth-child(2) { animation-delay: 0.2s; }
            .digitando span:nth-child(3) { animation-delay: 0.4s; }
            @keyframes piscar {
                0%, 80%, 100% { opacity: 0; }
                40% { opacity: 1; }
            }
        `).appendTo("head");
    });
