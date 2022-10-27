import 'bootstrap/dist/css/bootstrap.css';
// Put any other imports below so that CSS from your
// components takes precedence over default styles.
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import RegistrationComponent from './components/RegistrationComponent.js';
import MainCopmoment from './components/MainCompoment.js';
import PostCreateForm from './components/PostCreateForm.js';
import EnterComponent from './components/EnterComponent.js';

ReactDOM.render(<BrowserRouter><Routes>
    <Route path='/' element={<EnterComponent/>}/>
    <Route path='/registration' element={<RegistrationComponent/>}/>
    <Route path='/getPosts' element={<MainCopmoment/>}/> 
    <Route path='/addPost' element={<PostCreateForm/>}/>
    </Routes></BrowserRouter>, document.getElementById('root'));