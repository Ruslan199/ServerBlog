import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

export default function EnterComponent(props) {
    const [formData, setFormData] = useState({});
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const authorization = {
            Login: formData.login,
            Password: formData.password
        };

        const url = "https://localhost:5001/api/user/authorization";

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(authorization)

        })
            .then(response => response.json())
            .then(responseFromServer => {
                if(responseFromServer.token !== ""){
                    sessionStorage.setItem("token", responseFromServer.token);
                    sessionStorage.setItem("userId", responseFromServer.id);
                    navigate("/getPosts")
                }
            })
            .catch((error) => {
                console.log(error);
                alert(error);
            });
    };

    return (
        <div className="container">
            <div className="row min-vh-100">
                <div className="col d-flex flex-column justify-content-center align-items-center">
                    <form>
                        <h1>Добро пожаловать!</h1>

                        <div>
                            <label>Логин:</label>
                            <input value={formData.login} name="login" placeholder='Login' type="text" className="form-control" onChange={handleChange} />
                        </div>

                        <div>
                            <label>Пароль:</label>
                            <input value={formData.password} name="password" placeholder='Password' type="password" className="form-control" onChange={handleChange} />
                        </div>
                        <button onClick={handleSubmit} className="btn btn-dark btn-lg w-100 mt-5">Войти</button>
                        <button onClick={() => navigate("/registration")} className="btn btn-secondary btn-lg w-100 mt-3">Регистрация</button>
                    </form>
                </div>
            </div>
        </div>
    );
}
