<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="LojaVirtual.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pet World - Cuidando do seu pet onde ele estiver </title>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main_Content" runat="server">
    <div class="hero-wrap js-fullheight" style="background-image: url('images/bg_1.jpg');" data-stellar-background-ratio="0.5">
        <div class="overlay"></div>
        <div class="container">
            <div class="row no-gutters slider-text js-fullheight align-items-center justify-content-center" data-scrollax-parent="true">
                <div class="col-md-11 ftco-animate text-center">
                    <h1 class="mb-4">Cuidamos do seu Pet onde ele estiver </h1>
                    <div class="row no-gutters slider-text align-items-center justify-content-center">
                        <div class="d-flex " style="flex-direction: row; justify-content: center;">
                            <p><a href="prestadores.aspx" class="btn btn-primary mr-md-4 py-3 px-4">Buscar um Pet Sitter <span class="ion-ios-arrow-forward"></span></a></p>
                            <p><a href="criar_conta.aspx" class="btn btn-primary mr-md-4 py-3 px-4">Ser um Pet Sitter<span class="ion-ios-arrow-forward"></span></a></p>
                            <%--   <p>
                            <asp:Button ID="btn_buscarSitter" runat="server" Text="Buscar um Pet Sitter" class="btn btn-primary mr-md-4 py-3 px-4" OnClick="btn_buscarSitter_Click" /><span class="ion-ios-arrow-forward"></span></p>
                        <p>
                            <asp:Button ID="btn_serSitter" runat="server" Text="Quero ser um Pet Sitter" class="btn btn-primary mr-md-4 py-3 px-4" OnClick="btn_serSitter_Click" /><span class="ion-ios-arrow-forward"></span></p>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <section class="ftco-section bg-light ftco-no-pt ftco-intro">
        <div class="container">
            <div class="row">
                <div class="col-md-4 d-flex align-self-stretch px-4 ftco-animate">
                    <div class="d-block services active text-center">
                        <div class="icon d-flex align-items-center justify-content-center">
                            <span class="flaticon-blind"></span>
                        </div>
                        <div class="media-body">
                            <h3 class="heading">Dog Walking</h3>
                            <p>Passeios personalizados: levamos o seu cão para explorar parques e lugares ao redor, aproveitando ao máximo o que há na região.</p>
                            <a href="#" class="btn-custom d-flex align-items-center justify-content-center"><span class="fa fa-chevron-right"></span><i class="sr-only">Saber Mais</i></a>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 d-flex align-self-stretch px-4 ftco-animate">
                    <div class="d-block services text-center">
                        <div class="icon d-flex align-items-center justify-content-center">
                            <span class="flaticon-dog-eating"></span>
                        </div>
                        <div class="media-body">
                            <h3 class="heading">Pet Daycare</h3>
                            <p>
                                Cuidamos do seu cão durante o dia com segurança, carinho e diversão. Oferecemos um ambiente adaptado, atividades recreativas, socialização, descanso e alimentação conforme suas orientações. Enquanto você cuida da sua rotina, nós cuidamos do seu melhor amigo!/p>
                            <a href="#" class="btn-custom d-flex align-items-center justify-content-center"><span class="fa fa-chevron-right"></span><i class="sr-only">Saber mais</i></a>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 d-flex align-self-stretch px-4 ftco-animate">
                    <div class="d-block services text-center">
                        <div class="icon d-flex align-items-center justify-content-center">
                            <span class="flaticon-grooming"></span>
                        </div>
                        <div class="media-body">
                            <h3 class="heading">Banho & Tosquia</h3>
                            <p>Cuidamos da higiene e aparência do seu pet com carinho e segurança. Oferecemos banho com produtos adequados, tosquia higiênica ou estilizada, corte de unhas, limpeza de ouvidos e muito cuidado em cada detalhe.</p>
                            <a href="#" class="btn-custom d-flex align-items-center justify-content-center"><span class="fa fa-chevron-right"></span><i class="sr-only">Saber mais</i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="ftco-section ftco-no-pt ftco-no-pb">
        <div class="container">
            <div class="row d-flex no-gutters">
                <div class="col-md-5 d-flex">
                    <div class="img img-video d-flex align-self-stretch align-items-center justify-content-center justify-content-md-center mb-4 mb-sm-0" style="background-image: url(images/about-1.jpg);">
                    </div>
                </div>
                <div class="col-md-7 pl-md-5 py-md-5">
                    <div class="heading-section pt-md-5">
                        <h2 class="mb-4">Por que escolher a gente?</h2>
                    </div>
                    <div class="row">
                        <div class="col-md-6 services-2 w-100 d-flex">
                            <div class="icon d-flex align-items-center justify-content-center"><span class="flaticon-stethoscope"></span></div>
                            <div class="text pl-3">
                                <h4>Dicas de cuidados</h4>
                                <p>Garanta a saúde e o bem-estar do seu pet com atitudes simples: mantenha a vacinação em dia, ofereça alimentação equilibrada, água fresca, exercícios regulares, higiene adequada e visitas ao veterinário. E claro, muito amor e atenção todos os dias!</p>
                            </div>
                        </div>
                        <div class="col-md-6 services-2 w-100 d-flex">
                            <div class="icon d-flex align-items-center justify-content-center"><span class="flaticon-customer-service"></span></div>
                            <div class="text pl-3">
                                <h4>Apoio ao cliente</h4>
                                <p>Oferecemos um atendimento rápido, atencioso e humano. Estamos sempre disponíveis para tirar dúvidas, agendar serviços e garantir que você e seu pet tenham a melhor experiência possível.</p>
                            </div>
                        </div>
                        <div class="col-md-6 services-2 w-100 d-flex">
                            <div class="icon d-flex align-items-center justify-content-center"><span class="flaticon-emergency-call"></span></div>
                            <div class="text pl-3">
                                <h4>Serviços de Emergencia</h4>
                                <p>Atendimento rápido e eficiente em situações críticas, como acidentes, incêndios ou emergências médicas. Nossa equipe está disponível 24 horas por dia para garantir a segurança e o bem-estar da comunidade.</p>
                            </div>
                        </div>
                        <div class="col-md-6 services-2 w-100 d-flex">
                            <div class="icon d-flex align-items-center justify-content-center"><span class="flaticon-veterinarian"></span></div>
                            <div class="text pl-3">
                                <h4>Ajuda Veterinaria</h4>
                                <p>Suporte especializado para animais em situações de risco. Oferecemos atendimento emergencial, primeiros socorros e encaminhamento para clínicas parceiras, sempre priorizando o cuidado e a vida animal.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="ftco-counter" id="section-counter">
        <div class="container">
            <div class="row">
                <div class="col-md-6 col-lg-3 d-flex justify-content-center counter-wrap ftco-animate">
                    <div class="block-18 text-center">
                        <div class="text">
                            <strong class="number" data-number="50">0</strong>
                        </div>
                        <div class="text">
                            <span>Clientes</span>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 d-flex justify-content-center counter-wrap ftco-animate">
                    <div class="block-18 text-center">
                        <div class="text">
                            <strong class="number" data-number="8500">0</strong>
                        </div>
                        <div class="text">
                            <span>Prestadores</span>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 d-flex justify-content-center counter-wrap ftco-animate">
                    <div class="block-18 text-center">
                        <div class="text">
                            <strong class="number" data-number="20">0</strong>
                        </div>
                        <div class="text">
                            <span>Produtos</span>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 d-flex justify-content-center counter-wrap ftco-animate">
                    <div class="block-18 text-center">
                        <div class="text">
                            <strong class="number" data-number="50">0</strong>
                        </div>
                        <div class="text">
                            <span>Pets Cuidados</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="ftco-section testimony-section" style="background-image: url('images/bg_2.jpg');">
        <div class="overlay"></div>
        <div class="container">
            <div class="row justify-content-center pb-5 mb-3">
                <div class="col-md-7 heading-section text-center ftco-animate">
                    <h2>Clientes Felizes &amp; Feedbacks</h2>
                </div>
            </div>
            <div class="row ftco-animate">
                <div class="col-md-12">
                    <div class="carousel-testimony owl-carousel ftco-owl">
                        <%--    <div class="item">
                            <div class="testimony-wrap py-4">
                                <div class="icon d-flex align-items-center justify-content-center">
                                    <span class="fa fa-quote-left"></span>
                                </div>
                                <div class="text">
                                    <p class="mb-4">"Fiquei impressionada com a rapidez no agendamento e a atenção que deram ao meu pet. O veterinário foi super cuidadoso e explicou tudo com muita clareza. Me senti segura durante todo o atendimento."</p>
                                    <div class="d-flex align-items-center">
                                        <div class="user-img" style="background-image: url(images/person_1.jpg)"></div>
                                        <div class="pl-3">
                                            <p class="name">Camila Rodrigues</p>
                                            <span class="position">Tutora da Belinha</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>--%>

                        <div class="item">
                            <div class="testimony-wrap py-4">
                                <div class="icon d-flex align-items-center justify-content-center">
                                    <span class="fa fa-quote-left"></span>
                                </div>
                                <div class="text">
                                    <p class="mb-4">"Utilizei o serviço de pet sitter e fiquei impressionado com o carinho e atenção dedicados ao meu gato. Recebi atualizações e fotos diariamente."</p>
                                    <div class="d-flex align-items-center">
                                        <div class="user-img" style="background-image: url(images/person_2.jpg)"></div>
                                        <div class="pl-3">
                                            <p class="name">Felipe Mendes</p>
                                            <span class="position">Tutor do Mingau</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="item">
                            <div class="testimony-wrap py-4">
                                <div class="icon d-flex align-items-center justify-content-center">
                                    <span class="fa fa-quote-left"></span>
                                </div>
                                <div class="text">
                                    <p class="mb-4">"Levei minha cadela para banho e tosa e adorei o resultado! Equipe super atenciosa, ambiente limpo e seguro. Com certeza voltarei!"</p>
                                    <div class="d-flex align-items-center">
                                        <div class="user-img" style="background-image: url(images/person_3.jpg)"></div>
                                        <div class="pl-3">
                                            <p class="name">Juliana Lima</p>
                                            <span class="position">Tutora da Lola</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="item">
                            <div class="testimony-wrap py-4">
                                <div class="icon d-flex align-items-center justify-content-center">
                                    <span class="fa fa-quote-left"></span>
                                </div>
                                <div class="text">
                                    <p class="mb-4">"Foi reconfortante saber que, mesmo em um momento de emergência, pude contar com uma equipe profissional que realmente se importa com os animais."</p>
                                    <div class="d-flex align-items-center">
                                        <div class="user-img" style="background-image: url(images/person_4.jpg)"></div>
                                        <div class="pl-3">
                                            <p class="name">André Souza</p>
                                            <span class="position">Tutor do Max</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="item">
                            <div class="testimony-wrap py-4">
                                <div class="icon d-flex align-items-center justify-content-center">
                                    <span class="fa fa-quote-left"></span>
                                </div>
                                <div class="text">
                                    <p class="mb-4">"A consulta online foi prática e esclarecedora. Recebi orientação veterinária sem sair de casa e consegui resolver o problema do meu pet com segurança."</p>
                                    <div class="d-flex align-items-center">
                                        <div class="user-img" style="background-image: url(images/person_2.jpg)"></div>
                                        <div class="pl-3">
                                            <p class="name">Renata Alves</p>
                                            <span class="position">Tutora da Nina</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
