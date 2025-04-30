pipeline {
    agent { label 'ubuntu' }
    parameters {
        gitParameter defaultValue: 'origin/main', name: 'TAG', type: 'PT_BRANCH_TAG', sortMode: 'DESCENDING_SMART'
    }
	environment {
        GITHUB_TOKEN = credentials('GitHub-JSport')
    }
    stages {
        stage('Checkout') {
            steps {
                echo "Checkout ${params.TAG}"
                checkout scm
            }
        }
        stage('Build solution') {
            steps {
                sh 'dotnet build NetCrawlerDetect/NetCrawlerDetect.sln'
            }
        }
        stage('Test solution') {
            steps {
                sh 'dotnet test NetCrawlerDetect/NetCrawlerDetect.sln --logger "trx;LogFileName=unit_tests.xml"'
                xunit checksName: '', tools: [MSTest(excludesPattern: '', pattern: '**/TestResults/*.xml', stopProcessingIfError: true)]
            }
        }
        
    }
    
    post {
        success {
            slackSend channel: 'serverci', color: "good", message:"Build completed successfully - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
        }
        failure {
            slackSend channel: 'serverci', color: "danger", failOnError:true, message:"Build failed - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
        }
    }
}