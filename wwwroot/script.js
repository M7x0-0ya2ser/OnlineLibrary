// Auth
async function registerUser(userRegisterDto) {
  try {
    const response = await fetch('/api/v1/Register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userRegisterDto),
    });
    return await response.json();
  } catch (error) {
    console.error('Error registering user:', error);
       throw error;
  }
}

async function loginUser(userLoginDto) {
  try {
    const response = await fetch('/api/v1/Login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userLoginDto),
    });
    return await response.json();
  } catch (error) {
        console.error('Error logging in user:', error);
        throw error;
  }
}

// Books
async function getAllBooks() {
  try {
    const response = await fetch('/api/v1/books');
    return await response.json();
  } catch (error) {
        console.error('Error getting all books:', error);
   throw error;
  }
}

async function getBookByIsbn(isbn) {
  try {
    const response = await fetch(`/api/v1/books/${isbn}`);
    return await response.json();
  } catch (error) {
    console.error(`Error getting book by ISBN ${isbn}:`, error);
    throw error;
  }
}

async function addBook(book, token) { // Requires Admin role
  try {
    const response = await fetch('/api/v1/books', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(book),
    });
    return await response.json();
  } catch (error) {
        console.error('Error adding book:', error);
        throw error;
  }
}

async function deleteBook(isbn, token) { // Requires Admin role
  try {
    const response = await fetch(`/api/v1/books?isbn=${isbn}`, {  // Assuming query parameter for ISBN
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });
    return response.ok;
  } catch (error) {
    console.error(`Error deleting book with ISBN ${isbn}:`, error);
    throw error;
  }
}

async function editBook(book, token) { // Requires Admin role
  try {
    const response = await fetch('/api/v1/books', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(book),
    });
    return response.ok;
  } catch (error) {
    console.error(`Error editing book with ISBN ${book.isbn}:`, error);
    throw error;
  }
}

// BorrowedBooks
async function getBorrowedBooks() {
  try {
    const response = await fetch('/api/BorrowedBooks');
    return await response.json();
  } catch (error) {
    console.error('Error getting borrowed books:', error);
    throw error;
  }
}

async function getBorrowedBooksByUserId(userId) { // AllowAnonymous
  try {
    const response = await fetch(`/api/BorrowedBooks/${userId}`);
    return await response.json();
  } catch (error) {
    console.error(`Error getting borrowed books for user ID ${userId}:`, error);
    throw error;
  }
}

async function deleteBorrowedBook(orderNumber) {
  try {
    const response = await fetch(`/api/BorrowedBooks/${orderNumber}`, {
      method: 'DELETE',
    });
    return response.ok;
  } catch (error) {
    console.error(`Error deleting borrowed book with order number ${orderNumber}:`, error);
    throw error;
  }
}

async function addBorrowedBook(isbnDto) {
  try {
    const response = await fetch('/api/BorrowedBooks', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(isbnDto),
    });
    return response.ok;
  } catch (error) {
        console.error('Error adding borrowed book:', error);
        throw error;
  }
}

async function updateBorrowedBook(updateBorrowedBookDto) {
  try {
    const response = await fetch('/api/BorrowedBooks', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(updateBorrowedBookDto),
    });
    return response.ok;
  } catch (error) {
        console.error('Error updating borrowed book:', error);
        throw error;
  }
}

async function getCountBorrowedBook(token) { // Requires Admin role
  try {
    const response = await fetch('/api/BorrowedBooks/GetCountBorrowedBook', {
      headers: {
       'Authorization': `Bearer ${token}`,
        },
    });
    return await response.json();
  } catch (error) {
        console.error('Error getting count of borrowed books:', error);
        throw error;
  }
}

async function getAvailableBook(token) { // Requires Admin role
  try {
    const response = await fetch('/api/BorrowedBooks/GetAvailableBook', {
      headers: {
        'Authorization': `Bearer ${token}`,
        },
    });
    return await response.json();
  } catch (error) {
        console.error('Error getting available books:', error);
        throw error;
  }
}

// Users
async function getAllUsers() {
  try {
    const response = await fetch('/api/v1/users');
    return await response.json();
  } catch (error) {
        console.error('Error getting all users:', error);
        throw error;
  }
}

async function getUserById(id) {
  try {
    const response = await fetch(`/api/v1/users/${id}`);
    return await response.json();
  } catch (error) {
    console.error(`Error getting user by ID ${id}:`, error);
    throw error;
  }
}

async function updateUser(id, updateuserDto, token) { // Requires Admin role
  try {
    const response = await fetch(`/api/v1/users/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(updateuserDto),
    });
    return response.ok;
  } catch (error) {
        console.error(`Error updating user with ID ${id}:`, error);
        throw error;
  }
}
