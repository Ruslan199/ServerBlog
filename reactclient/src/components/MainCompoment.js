import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { deletePost } from './PostCreateForm.js';

let isAllPostsFromServer = false;

export default function MainCopmonent(props) {
  
  const [posts, setPosts] = useState([]);
  const navigate = useNavigate();
  isAllPostsFromServer = false;

  function getPosts() {
    const url = "https://localhost:5001/api/posts/getUserPosts";

    fetch(url,{
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
      .then(response => response.json())
      .then(postsFromServer => {
        setPosts(postsFromServer.posts);
      })
      .catch((error) => {
        if(error)
        navigate("/");
      });
  }

  function getAllPosts() {
    const url = "https://localhost:5001/api/posts/getAllPosts";
    isAllPostsFromServer = true;

    fetch(url,{
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
      .then(response => response.json())
      .then(postsFromServer => {
        setPosts(postsFromServer.posts);
      })
      .catch((error) => {
        if(error)
        navigate("/");
      });
  }

  function exit() {
      sessionStorage.removeItem("token");
      sessionStorage.removeItem("userId");
      navigate("/login");
  }

  function RenderPostTable({posts, setPosts}) {
    return (
      <div className="table-responsive mt-5">
          <table className="table table-bordered border-dark">
              <thead>
                  <tr>
                    <th scope="col">PostId PK</th>
                    { !isAllPostsFromServer && <th scope="col">Автор</th>}
                    <th scope="col">Заголовок</th>
                    <th scope="col">Контент</th>
                    <th scope="col">Дата создания</th>
                    { !isAllPostsFromServer &&<th scope="col">Дейтсвия</th>}
                  </tr>
              </thead>
              <tbody>
                {posts.map((post) => (
                  <tr key={post.postId}>
                    <th scope="row">{post.postId}</th>
                    {!isAllPostsFromServer && <td>{post.author}</td>}
                    <td>{post.title}</td>
                    <td>{post.content}</td>
                    <td>{post.createdOn}</td>
                    {!isAllPostsFromServer &&
                      <td>
                        <button onClick={ () => {props.setPost(post); navigate("/updatePost") } } className="btn btn-dark btn-lg mx-3 my-3">Обновить</button>
                        <button onClick={ () => deletePost(post.postId) } className="btn btn-secondary btn-lg">Удалить</button>
                      </td>
                    }
                  </tr>
                ))}
              </tbody>
          </table>
  
          <button onClick={() => setPosts([])} className="btn btn-dark btn-lg w-100">Очистить посты</button>
      </div>
    );
  }

  useEffect(() => getPosts(),[]);
  return (
    <div className="container">
       <div className="">
          <div className="btn-group" role="group" aria-label="Basic example">
              <button onClick={() => navigate("/addPost")} className="btn btn-secondary btn-lg w-10 mt-4">Создать новый пост</button>
          </div>
          <div className="btn-group" role="group" aria-label="Basic example">
              <button onClick={ getPosts } className="btn btn-secondary btn-lg w-100 mt-4">Получить мои посты</button>
          </div>
          <div className="btn-group" role="group" aria-label="Basic example">
              <button onClick={ getAllPosts } className="btn btn-secondary btn-lg w-100 mt-4">Получить посты других пользователей</button>
          </div>
          <div className="btn-group col-md-4 col-md-offset-4" role="group" aria-label="Basic example">
              <button onClick={ exit } className="btn btn-secondary btn-lg w-20 mt-4 pull-right">Выйти</button>
          </div>
      </div>
      <div className="row min-vh-10">
        <div className="col d-flex flex-column justify-content-center align-items-center">
          {posts.length > 0 && <RenderPostTable posts={posts} setPosts={setPosts}/>}
        </div>
      </div>    
    </div>
  );
}