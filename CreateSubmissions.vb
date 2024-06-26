﻿Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text
Imports System.Diagnostics

Public Class CreateSubmissions

    Private stopwatch As Stopwatch = New Stopwatch()
    Private isStopwatchRunning As Boolean = False
    Private WithEvents timer As Timer

    Private Sub CreateSubmissions_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.T Then
            StopwatchToggle()
        End If

        If e.Control AndAlso e.KeyCode = Keys.S Then
            CreateSubmissionsMethod()
        End If
    End Sub

    Private Sub CreateSubmissions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        timer = New Timer()
        timer.Interval = 50 ' 1000 milliseconds = 1 second
        AddHandler timer.Tick, AddressOf Timer_Tick
        ' Start stopwatch when form loads

        stopwatch.Start()
        isStopwatchRunning = True
        timer.Start()
        UpdateStopwatchLabel()
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs)
        UpdateStopwatchLabel()
    End Sub

    Private Sub CreateSubmissions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Stop stopwatch when form is closing
        stopwatch.Stop()
        isStopwatchRunning = False
        timer.Stop()
    End Sub

    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        stopwatchToggle()

    End Sub

    Private Sub StopwatchToggle()
        If isStopwatchRunning Then
            stopwatch.Stop()
            isStopwatchRunning = False
            timer.Stop()
        Else
            stopwatch.Start()
            isStopwatchRunning = True
            timer.Start()
        End If

        UpdateStopwatchLabel()
    End Sub

    Private Sub UpdateStopwatchLabel()
        labelStopwatchTime.Text = stopwatch.Elapsed.ToString("mm\:ss\:ff")
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        CreateSubmissionsMethod()
    End Sub

    Private Async Sub CreateSubmissionsMethod()
        Dim submission = New With {
            .Name = textName.Text,
            .Email = textEmail.Text,
            .Phone = textPhone.Text,
            .GithubLink = textGithub.Text,
            .StopwatchTime = labelStopwatchTime.Text
        }

        Dim client As New HttpClient()
        client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
        Dim jsonData = JsonConvert.SerializeObject(submission)
        Dim content = New StringContent(jsonData, System.Text.Encoding.UTF8, "application/json")

        Try
            Dim response = Await client.PostAsync("http://localhost:3000/submit", content)
            Dim responseBody = Await response.Content.ReadAsStringAsync()

            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission successful!")
            Else
                MessageBox.Show($"Submission failed: {responseBody}")
            End If
        Catch ex As Exception
            MessageBox.Show($"An error occurred: {ex.Message}")
        End Try
    End Sub


End Class
