function showToast(mensagem, tipo = "sucesso") {
    Toastify({
        text: mensagem,
        duration: 4000,
        gravity: "top",
        position: "right",
        style: {
            background: tipo === "erro" ? "#e74c3c" : "#27ae60",
            color: "#fff",
            borderRadius: "5px",
        },
        close: true,
    }).showToast();
}

// -------------------- LOGIN --------------------
var loginPage = document.getElementById('loginPage');

if (loginPage) {
    const togglePassword = document.getElementById('togglePassword');
    const passwordInput = document.getElementById('Senha');
    const btnLogin = document.getElementById('btnLogin');

    togglePassword.addEventListener('click', function () {
        const type = passwordInput.type === 'password' ? 'text' : 'password';
        passwordInput.type = type;
        this.querySelector('i').classList.toggle('fa-eye');
        this.querySelector('i').classList.toggle('fa-eye-slash');
    });

    btnLogin.addEventListener('click', async function (e) {
        e.preventDefault();

        const email = document.getElementById('Email').value;
        const senha = document.getElementById('Senha').value;

        if (!email || !senha) {
            showToast("Por favor, preencha o email e a senha.", "erro");
            return;
        }

        try {
            const response = await fetch('/Autenticar', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, senha })
            });

            const data = await response.json();

            if (response.ok && data.sucesso) {
                showToast("Login realizado com sucesso.");
                setTimeout(() => {
                    window.location.href = "/Home/Index";
                }, 1500);
            } else {
                showToast(data.mensagem || "Erro ao realizar login.", "erro");
            }
        } catch (error) {
            console.error("Erro na requisição de login:", error);
            showToast("Erro ao se conectar com o servidor.", "erro");
        }
    });
}

// -------------------- CADASTRO ADMINISTRADOR --------------------
var registerPage = document.getElementById('registerAccountPage');

if (registerPage) {
    const form = document.getElementById('registerForm');
    const togglePassword = document.getElementById('togglePassword');
    const senhaInput = document.getElementById('Senha');

    togglePassword.addEventListener('click', function () {
        const type = senhaInput.type === 'password' ? 'text' : 'password';
        senhaInput.type = type;
        this.querySelector('i').classList.toggle('fa-eye');
        this.querySelector('i').classList.toggle('fa-eye-slash');
    });

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        const nome = document.getElementById('Nome').value;
        const email = document.getElementById('Email').value;
        const senha = document.getElementById('Senha').value;

        if (!nome || !email || !senha) {
            showToast("Preencha todos os campos.", "erro");
            return;
        }

        try {
            const response = await fetch('/Account/CriarUsuario', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ nome, email, senha })
            });

            const data = await response.json();

            if (response.ok && data.sucesso) {
                showToast("Administrador criado com sucesso.");
                setTimeout(() => {
                    window.location.href = "/Login/Index";
                }, 1500);
            } else {
                showToast(data.mensagem || "Erro ao criar administrador.", "erro");
            }
        } catch (error) {
            console.error("Erro ao criar administrador:", error);
            showToast("Erro de conexão com o servidor.", "erro");
        }
    });
}

var homePage = document.getElementById('homePage');
var modalCadastrarUsuario = document.getElementById('modalCadastrarUsuario');
var modalReconhecimento = document.getElementById('modalReconhecimentoFacial');

if (homePage) {
    let vetorFacialGlobal = "";
    let fotoBase64 = "";
    let stream = null;
    let detectionInterval = null;
    let reconhecimentoFeito = false;

    async function carregarFaceAPI() {
        const url = 'https://justadudewhohacks.github.io/face-api.js/models';
        await faceapi.nets.tinyFaceDetector.loadFromUri(url);
        await faceapi.nets.faceRecognitionNet.loadFromUri(url);
        await faceapi.nets.faceLandmark68Net.loadFromUri(url);
    }

    async function iniciarVideo(videoId = 'video') {
        stream = await navigator.mediaDevices.getUserMedia({ video: {} });
        const video = document.getElementById(videoId);
        video.srcObject = stream;
        video.play();
    }

    function pararVideo() {
        clearInterval(detectionInterval);
        if (stream) stream.getTracks().forEach(track => track.stop());
    }

    function capturarFace(videoId = 'video', canvasId = 'canvas', previewId = 'previewImage', previewContainerId = 'previewContainer') {
        const video = document.getElementById(videoId);
        const canvas = document.getElementById(canvasId);
        const previewImage = document.getElementById(previewId);
        const previewContainer = document.getElementById(previewContainerId);

        const ctx = canvas.getContext('2d');
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

        fotoBase64 = canvas.toDataURL("image/jpeg");
        previewImage.src = fotoBase64;
        previewContainer.classList.remove("d-none");
    }

    // Cadastro
    window.retirarFoto = function () {
        reconhecimentoFeito = false;
        vetorFacialGlobal = "";
        fotoBase64 = "";
        document.getElementById('previewContainer').classList.add("d-none");
        showToast("Você pode posicionar o rosto novamente para nova captura.");
    };

    window.salvarUsuario = async function () {
        const nome = document.getElementById('Nome')?.value;
        const documento = document.getElementById('Documento')?.value;
        const email = document.getElementById('Email')?.value;
        const tipo = document.getElementById('Tipo')?.value;
        const endereco = document.getElementById('Endereco')?.value;
        const obs = document.getElementById('Observacao')?.value;

        if (!nome || !documento || !vetorFacialGlobal.length) {
            showToast("Preencha os dados obrigatórios e capture o rosto.", "erro");
            return;
        }

        const body = {
            nome,
            documento,
            email,
            tipo,
            enderecoResidencial: tipo === "0" ? endereco : null,
            observacao: tipo === "1" ? obs : null,
            fotoBase64,
            vetorFacial: JSON.stringify(vetorFacialGlobal)
        };

        try {
            const res = await fetch('/Usuario/CadastrarViaModal', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });

            const data = await res.json();
            if (res.ok && data.sucesso) {
                showToast("Usuário cadastrado com sucesso.");
                document.getElementById('formCadastroUsuario')?.reset();
                vetorFacialGlobal = "";
                fotoBase64 = "";
                document.getElementById('previewContainer')?.classList.add("d-none");
                const modal = bootstrap.Modal.getInstance(modalCadastrarUsuario);
                modal?.hide();
            } else {
                showToast(data.mensagem || "Erro ao cadastrar usuário.", "erro");
            }
        } catch (e) {
            console.error("Erro no cadastro:", e);
            showToast("Erro de conexão com o servidor.", "erro");
        }
    };

    // Tipo de usuário muda campos visíveis
    document.getElementById('Tipo')?.addEventListener('change', function () {
        const tipo = this.value;
        document.getElementById('EnderecoDiv')?.style.setProperty('display', tipo === "0" ? "block" : "none");
        document.getElementById('ObservacaoDiv')?.style.setProperty('display', tipo === "1" ? "block" : "none");
    });

    // Modal Cadastro
    modalCadastrarUsuario?.addEventListener('shown.bs.modal', async () => {
        await carregarFaceAPI();
        await iniciarVideo('video');

        detectionInterval = setInterval(async () => {
            if (!reconhecimentoFeito) {
                const video = document.getElementById('video');
                const result = await faceapi
                    .detectSingleFace(video, new faceapi.TinyFaceDetectorOptions())
                    .withFaceLandmarks()
                    .withFaceDescriptor();

                if (result) {
                    capturarFace('video', 'canvas', 'previewImage', 'previewContainer');
                    vetorFacialGlobal = Array.from(result.descriptor);
                    reconhecimentoFeito = true;
                    showToast("Rosto capturado com sucesso.");
                }
            }
        }, 1000);
    });

    modalCadastrarUsuario?.addEventListener('hidden.bs.modal', () => {
        document.getElementById('formCadastroUsuario')?.reset();
        vetorFacialGlobal = "";
        fotoBase64 = "";
        reconhecimentoFeito = false;
        pararVideo();
        document.getElementById('previewContainer')?.classList.add("d-none");
    });

    // Modal Reconhecimento
    modalReconhecimento?.addEventListener('shown.bs.modal', async () => {
        await carregarFaceAPI();
        await iniciarVideo('videoReconhecimento');

        detectionInterval = setInterval(async () => {
            if (!reconhecimentoFeito) {
                const video = document.getElementById('videoReconhecimento');
                const result = await faceapi
                    .detectSingleFace(video, new faceapi.TinyFaceDetectorOptions())
                    .withFaceLandmarks()
                    .withFaceDescriptor();

                if (result) {
                    pararVideo();
                    reconhecimentoFeito = true;
                    vetorFacialGlobal = Array.from(result.descriptor);
                    showToast("Rosto detectado. Verificando...");

                    const res = await fetch('/Usuario/ReconhecerFace', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ vetorFacial: vetorFacialGlobal }) // ENVIO CORRETO
                    });

                    const data = await res.json();
                    if (res.ok && data.sucesso) {
                        document.getElementById('nomeReconhecido').innerText = data.usuario.nome;
                        document.getElementById('tipoReconhecido').innerText = data.usuario.tipo === 0 ? "Morador" : "Visitante";
                        document.getElementById('enderecoOuObsReconhecido').innerText = data.usuario.enderecoResidencial || data.usuario.observacao || "-";
                        document.getElementById('reconhecimentoResultado')?.classList.remove("d-none");
                    } else {
                        showToast(data.mensagem || "Usuário não reconhecido", "erro");
                    }
                }
            }
        }, 1000);
    });

    modalReconhecimento?.addEventListener('hidden.bs.modal', () => {
        vetorFacialGlobal = "";
        reconhecimentoFeito = false;
        pararVideo();
        document.getElementById('reconhecimentoResultado')?.classList.add("d-none");
    });

    // Chamada manual se precisar
    window.iniciarReconhecimentoFacial = function () {
        const modal = new bootstrap.Modal(modalReconhecimento);
        modal.show();
    };
}
