module.exports = function (app) {



    //POST
    addUsuario = function (req, res) {
        res.send(req.body.home);
    };

    //API Routes Usuarios
    app.post('/usuarios', addUsuario);
}