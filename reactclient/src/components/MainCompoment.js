import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

export default function MainCopmonent(props) {
  
  const [posts, setPosts] = useState([]);
  const [countPosts, setCountPosts] = useState([]);
  const [isUserPost, setisUserPost] = useState(true);
  const navigate = useNavigate();

  function getUserPosts() {
    const url = "https://localhost:5001/api/posts/getUserPosts";

    fetch(url,{
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
      .then(response => response.json())
      .then(postsFromServer => {
        if(postsFromServer.success) {
          setPosts(postsFromServer.posts);
          setisUserPost(true);
        }
      })
      .catch((error) => {
        alert(error);
      });
  }

  function getCountPosts() {
    const url = "https://localhost:5001/api/posts/getCountPosts";

    fetch(url,{
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
      .then(response => response.json())
      .then(postsFromServer => {
        if(postsFromServer.success) {
          setCountPosts(postsFromServer.allPosts);
        }
      })
      .catch((error) => {
        alert(error);
      });
  }

  function getAllPosts() {
    const url = "https://localhost:5001/api/posts/getAllPosts";

    fetch(url,{
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
      .then(response => response.json())
      .then(postsFromServer => {
        if(postsFromServer.success) {
          setPosts(postsFromServer.posts);
          setisUserPost(false);
        }
      })
      .catch((error) => {
        alert(error);
      });
  }

  function exit() {
      sessionStorage.removeItem("token");
      sessionStorage.removeItem("userId");
      navigate("/login");
  }

  function deletePost(postId) {
    const url = "https://localhost:5001/api/posts/" + postId;
  
    fetch(url,{
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
    .then(response => {
        if(response.status === 204){
          getUserPosts();
          getCountPosts();
        }
    })
    .catch((error) => {
        alert(error);
    });
}

  function RenderPostTable({posts, setPosts, isUserPost}) {
    return (
      <div className="table-responsive mt-5">
        <h1>{!isUserPost ? "Посты других пользователей" : "Мои посты"}</h1>
          <table className="table table-bordered border-dark">
              <thead>
                  <tr>
                    <th scope="col">PostId PK</th>
                    { !isUserPost && <th scope="col">Автор</th>}
                    <th scope="col">Заголовок</th>
                    <th scope="col">Контент</th>
                    <th scope="col">Дата создания</th>
                    { isUserPost &&<th scope="col">Действия</th>}
                  </tr>
              </thead>
              <tbody>
                {posts.map((post) => (
                  <tr key={post.postId}>
                    <th scope="row">{post.postId}</th>
                    { !isUserPost && <td>{post.author}</td>}
                    <td>{post.title}</td>
                    <td>{post.content}</td>
                    <td>{post.createdOn}</td>
                    { isUserPost &&
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

  function RenderCountPostTable() {
    return (
      <div className="table-responsive mt-5">
        <h1>Количество постов всех пользователей</h1>
          <table className="table table-bordered border-dark">
              <thead>
                  <tr>
                    <th scope="col">Имя пользователя</th>
                    <th scope="col">Количество постов</th>
                  </tr>
              </thead>
              <tbody>
                {countPosts.map((post) => (
                    <tr key={post.countPost}>
                      <th scope="row">{post.userName}</th>
                      <td>{post.countPost}</td>
                    </tr>
                ))}
              </tbody>
          </table>
  
          <button onClick={() => setCountPosts([])} className="btn btn-dark btn-lg w-100">Очистить таблицу</button>
      </div>
    );
  }

  useEffect(() => getUserPosts(),[]);
  useEffect(() => getCountPosts(),[]);
  return (
    <div className="container">
       <div className="">
          <div className="btn-group" role="group" aria-label="Basic example">
              <button onClick={ () => navigate("/addPost") } className="btn btn-secondary btn-lg w-10 mt-4">Создать новый пост</button>
          </div>
          <div className="btn-group" role="group" aria-label="Basic example">
              <button onClick={ getUserPosts } className="btn btn-secondary btn-lg w-100 mt-4">Получить мои посты</button>
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
          { posts.length > 0 && <RenderPostTable posts={posts} setPosts={ setPosts } isUserPost= { isUserPost }/> }
          { countPosts.length > 0 && <RenderCountPostTable countPosts={countPosts} setCountPosts={ setCountPosts } /> }
        </div>
      </div>    
    </div>
  );
}