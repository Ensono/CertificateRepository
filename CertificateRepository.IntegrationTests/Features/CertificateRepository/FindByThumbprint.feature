Feature: Certificate Repository Find By Thumbprint
	Must throw exception when a thumbprint starts or ends with a left-to-right-mark (U+200E)
	Must throw exception when a thumbprint starts or ends wtih a space
	Must throw exception when a thumbprint contains an invalid character
	Must throw exception when a thumbprint is of the wrong length

  @CleanupCertificatesOnError
  Scenario: Throw exeption when thumbprint starts with a left-to-right-mark
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C' prefixed with a 'left-to-right-mark'
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 3 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Throw exeption when thumbprint ends with a left-to-right-mark
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C' sufixed with a 'left-to-right-mark'
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 3 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Throw exeption when thumbprint starts with a space
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C' prefixed with a 'space'
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 2 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Throw exeption when thumbprint ends with a space
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C' sufixed with a 'space'
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 2 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario Outline: Throw exeption when thumbprint contains an invalid character
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint '<Thumbprint>' from the following examples
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 1 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up
	Examples: 
	| Thumbprint                               |
	| EFFF7FD55F1F3 7B67CCE6F539071AD13A4BFA92 |
	| EFFF7FD55F1F37B67CCE6F539071XAD13A4BFA92 |
	| EFFF7FD55F1F37B67CCE6F539071'AD13A4BFA92 |
	| EF-FF7FD55F1F37B67CCE6F539071AD13A4BFA92 |

  @CleanupCertificatesOnError
  Scenario Outline: Throw exeption when thumbprint is of an incorrect length
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint '<Thumbprint>' from the following examples
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 1 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up
	Examples: 
	| Thumbprint                                                    |
	| EFFF7FD55F1F37B67CCE6F539071AD13A4BFA9C                       |
	| E                                                             |
	| EFFF7FD55F1F37B67CC                                           |
	| EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92CA                     |
	| EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92CE6F539071AD13A4BFA92C |
