package eventures.data;

public class UserRegisterModel {
    private String username;
    private String email;
    private String password;
    private String confirmPassword;
    private String firstName;
    private String lastName;

    public void setUsername(String username) {
        this.username = username;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public void setConfirmPassword(String confirmPassword) { this.confirmPassword = confirmPassword; }

    public void setFirstName(String firstName) { this.firstName = firstName; }

    public void setLastName(String lastName) { this.lastName = lastName; }
}
