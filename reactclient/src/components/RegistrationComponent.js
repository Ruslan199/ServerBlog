import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

export default function RegistrationComponent(props) {
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

        const userToCreate = {
            Login: formData.login,
            Email: formData.email,
            Password: formData.password
        };

        const url = "https://localhost:5001/api/user/registration";

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userToCreate)

        })
            .then(response => response.json())
            .then(responseFromServer => {
                if(responseFromServer.success) {
                    alert(responseFromServer.message);
                    navigate('/login');
                }
                else{
                    alert(responseFromServer.message);
                }
            })
            .catch((error) => {
                console.log(error);
                alert(error);
            });
    };

    return (
        <form className="w-100 px-5">
            <h1 className="mt-5">Регистрация</h1>

            <div className="mt-5">
                <label className="h3 form-label">Логин:</label>
                <input value={formData.login} name="login" type="text" className="form-control" onChange={handleChange} />
            </div>

            <div className="mt-4">
                <label className="h3 form-label">Пароль:</label>
                <input value={formData.password} name="password" type="password" className="form-control" onChange={handleChange} />
            </div>

            <div className="mt-4">
                <label className="h3 form-label">Почта:</label>
                <input value={formData.email} name="email" type="text" className="form-control" onChange={handleChange} />
            </div>

            <button onClick={handleSubmit} className="btn btn-dark btn-lg w-100 mt-5">Регистрация</button>
            <button onClick={() => navigate("/")} className="btn btn-secondary btn-lg w-100 mt-3">Закрыть</button>
        </form>
    );
}
