@raw_input = ''
@inp_register = {}
inp = File.readlines('i.txt')
@raw_input = inp[0]


def checkPair(current, next_c)
  # check if they are the same character
  # puts "Checking: " + current + " with " + next_c
  return unless current.casecmp(next_c).zero?

  current != next_c
end

def snip(input, a)
  (0..input.length).each do |i|
    current = input[i]

    return unless i != input.length-1

    next_c = input[i+1]

    next if next_c.nil?

    if checkPair(current, next_c)
      # ok need to cut current and next_c out of string
      @inp_register[a].slice!(i,2)
    end
  end
end

lowest = 1 << 64
('a'..'z').each do |a|
  raw = @raw_input
  raw_l_case_trimmed = raw.tr(a,'')
  aup = a.upcase
  raw_all_trimmed = raw_l_case_trimmed.tr(aup, '')


  @inp_register[a] = raw_all_trimmed
  (0..10_000).each do
    snip(@inp_register[a], a)
  end

  puts a + ': ' + @inp_register[a].length.to_s

  lowest = @inp_register[a].length if @inp_register[a].length < lowest
end

puts lowest







