import { render, screen, fireEvent } from '@testing-library/react';
import Register from '../../components/Register/Register'
import '../../i18nextInit'

jest.mock('../../services/UserService')

// Verify that the email input was rendered
test("render email input", () => {
    render(<Register />)
    const element = screen.getByPlaceholderText("Email")
    expect(element).toBeInTheDocument()
})

// Verify that the name input was rendered
test("render name input", () => {
    render(<Register />)
    const element = screen.getByPlaceholderText("Name")
    expect(element).toBeInTheDocument()
})

// Verify that the password input was rendered
test("render password input", () => {
    render(<Register />)
    const element = screen.getByPlaceholderText("Password")
    expect(element).toBeInTheDocument()
})

// Verify that the confirm password input was rendered
test("render confirm password input", () => {
    render(<Register />)
    const element = screen.getByPlaceholderText("Confirm Password")
    expect(element).toBeInTheDocument()
})

// Verify that the tags input was rendered
test("render tags input", () => {
    render(<Register />)
    const element = screen.getByPlaceholderText("Add a tag")
    expect(element).toBeInTheDocument()
})

// Verify that profile information button was rendered
test("render profile information button", () => {
    render(<Register />)
    const element = screen.getByRole('button')
    expect(element).toBeInTheDocument()
})

// Verify that avatar input was rendered after clicking button
test("render avatar input", () => {
    render(<Register />)
    const btn = screen.getByRole('button')
    fireEvent.click(btn)
    const element = screen.getByPlaceholderText("Avatar Url")
    expect(element).toBeInTheDocument()
})

// Verify that phone number input was rendered after clicking button
test("render phone number input", () => {
    render(<Register />)
    const btn = screen.getByRole('button')
    fireEvent.click(btn)
    const element = screen.getByPlaceholderText("Telephone Number")
    expect(element).toBeInTheDocument()
})

// Verify that description input was rendered after clicking button
test("render description input", () => {
    render(<Register />)
    const btn = screen.getByRole('button')
    fireEvent.click(btn)
    const element = screen.getByPlaceholderText("Profile Description")
    expect(element).toBeInTheDocument()
})

// Verify that linkedin input was rendered after clicking button
test("render linkedin input", () => {
    render(<Register />)
    const btn = screen.getByRole('button')
    fireEvent.click(btn)
    const element = screen.getByPlaceholderText("Linkedin")
    expect(element).toBeInTheDocument()
})

// Verify that facebook input was rendered after clicking button
test("render facebook input", () => {
    render(<Register />)
    const btn = screen.getByRole('button')
    fireEvent.click(btn)
    const element = screen.getByPlaceholderText("Facebook")
    expect(element).toBeInTheDocument()
})

// Verify that submit button was rendered after clicking button
test("render submit button", () => {
    render(<Register />)
    const btn = screen.getByRole('button')
    fireEvent.click(btn)
    const element = screen.getAllByRole('button')[1]
    expect(element).toBeInTheDocument()
})